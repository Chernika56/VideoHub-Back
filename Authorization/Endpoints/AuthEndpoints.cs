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
            builder.MapPost("/logout", UserLogout)
                .WithOpenApi();
            builder.MapGet("/whoami", WhoAmI)
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
                    //Secure = false,
                    //SameSite = SameSiteMode.Unspecified
                });
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest();
            }
        }

        [Authorize]
        private static IResult UserLogout(HttpContext context)
        {
            context.Response.Cookies.Delete("jwtToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
                //Secure = false,
                //SameSite = SameSiteMode.Unspecified
            });

            return Results.Ok(new { message = "Successfully logged out" });
        }

        [Authorize]
        private async static Task<IResult> WhoAmI([FromServices] AuthService service)
        {
            var profile = await service.WhoAmI();

            return profile is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(profile);
        }
    }
}
