using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.Services;
using BackEnd.Users.DTO.RequestDTO;
using BackEnd.Users.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Users.Endpoints
{ 
    public static class UserEndpoints
    {
        public static void MapUsers(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetUsers)
                .WithOpenApi();
            builder.MapGet("/{userId:int}", GetUser)
                .WithOpenApi();
            builder.MapPost("/", CreateUser)
                .WithOpenApi();
            builder.MapPut("/{userId:int}", ChangeUser)
                .WithOpenApi();
            builder.MapDelete("/{userId:int}", DeleteUser)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> GetUsers([FromServices] UserService service)
        {
            var users = await service.GetUsers();

            return users is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(users);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetUser([FromServices] UserService service, int userId)
        {
            var user = await service.GetUser(userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> CreateUser([FromServices] UserService service, [FromBody] CreateUserDTO dto)
        {
            var user = await service.CreateUser(dto);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeUser([FromServices] UserService service, [FromBody] UserRequestDTO dto, int userId)
        {
            var user = await service.ChangeUser(dto, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteUser([FromServices] UserService service, int userId)
        {
            var res = await service.DeleteUser(userId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
