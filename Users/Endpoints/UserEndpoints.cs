using BackEnd.Users.DTO;
using BackEnd.Users.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BackEnd.Users.Endpoints
{ 
    public static class UserEndpoints
    {
        public static void MapUser(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/login", UserAuthentication)
                .WithOpenApi();
            builder.MapPost("/registration", RegisterUser)
                .WithOpenApi();
        }

        private async static Task<IResult> UserAuthentication(HttpContext context, [FromServices] UserService service, [FromBody] AuthenticationDTO dto)
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

        private async static Task<IResult> RegisterUser(HttpContext context, [FromServices] UserService service, [FromBody] RegistrationDTO dto)
        {
            var token = await service.RegisterUser(dto);

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
