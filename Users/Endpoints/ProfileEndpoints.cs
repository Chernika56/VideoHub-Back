using BackEnd.Users.DTO.RequestDTO;
using BackEnd.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Users.Endpoints
{
    public static class ProfileEndpoints
    {
        public static void MapProfile(this IEndpointRouteBuilder builder)
        {
            //builder.MapPost("/registration", RegisterUser)
            //    .WithOpenApi();
            builder.MapGet("/", GetProfile)
                .WithOpenApi();
            builder.MapPut("/", ChangeProfile)
                .WithOpenApi();
            builder.MapGet("/organizations", GetMyOrganizations)
                .WithOpenApi();
            builder.MapGet("/organizations/{organization_id:uint}/folders", GetMyFoldersInOrganization)
                .WithOpenApi();
            builder.MapGet("/folders", GetMyFolders)
                .WithOpenApi();
            builder.MapGet("/messages", GetMyMessages)
                .WithOpenApi();
        }

        [Authorize]
        private async static Task<IResult> GetProfile([FromServices] ProfileService service)
        {
            var profile = await service.GetProfile();

            return profile is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(profile);
        }

        [Authorize]
        private async static Task<IResult> ChangeProfile([FromServices] ProfileService service, [FromBody] ProfileRequestDTO dto)
        {
            var profile = await service.ChangeProfile(dto);

            return profile is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(profile);
        }

        [Authorize]
        private async static Task<IResult> GetMyOrganizations([FromServices] ProfileService service)
        {
            var organizations = await service.GetMyOrganizations();

            return organizations is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(organizations);
        }

        [Authorize]
        private async static Task<IResult> GetMyFoldersInOrganization([FromServices] ProfileService service, uint organization_id)
        {
            var folders = await service.GetMyFoldersInOrganization(organization_id);

            return folders is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folders);
        }

        [Authorize]
        private async static Task<IResult> GetMyFolders([FromServices] ProfileService service)
        {
            var folders = await service.GetMyFolders();

            return folders is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folders);
        }

        [Authorize]
        private async static Task<IResult> GetMyMessages([FromServices] ProfileService service)
        {
            var messages = await service.GetMyMessages();

            return Results.Ok(messages);
        }
    }
}
