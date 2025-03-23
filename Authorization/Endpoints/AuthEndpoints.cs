using BackEnd.Authorization.DTO;
using BackEnd.Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Authorization.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuth(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/login", UserAuthentication)
                .WithOpenApi();
        }

        private async static Task<IResult> UserAuthentication(HttpContext context, [FromServices] AuthService service, [FromBody] AuthenticationDTO dto)
        {
            var token = await service.AuthenticationUser(dto);

            if (token != null)
            {
                context.Response.Cookies.Append("jwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest();
            }
        }
    }
}
