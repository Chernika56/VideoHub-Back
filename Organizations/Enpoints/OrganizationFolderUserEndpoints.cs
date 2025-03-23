using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Organizations.Enpoints
{
    public static class OrganizationFolderUserEndpoints
    {
        public static void MapOrganizationFolderUsers(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{organizationId:uint}/folders/{folderId:uint}/users", GetFolderUsers)
                .WithOpenApi();
            builder.MapGet("/{organizationId:uint}/folders/{folderId:uint}/users/{userId:uint}", GetFolderUser)
                .WithOpenApi();
            builder.MapPut("/{organizationId:uint}/folders/{folderId:uint}/users/{userId:uint}", ChangeFolderUser)
                .WithOpenApi();
            builder.MapDelete("/{organizationId:uint}/folders/{folderId:uint}/users/{userId:uint}", DeleteFolderUser)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetFolderUsers([FromServices] OrganizationFolderUserService service, uint organizationId, uint folderId)
        {
            var users = await service.GetFolderUsers(organizationId, folderId);

            return users is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(users);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetFolderUser([FromServices] OrganizationFolderUserService service, uint organizationId, uint folderId, uint userId)
        {
            var user = await service.GetFolderUser(organizationId, folderId, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeFolderUser([FromServices] OrganizationFolderUserService service, [FromBody] FolderUserRequestDTO dto, uint organizationId, uint folderId, uint userId)
        {
            var user = await service.ChangeFolderUser(dto, organizationId, folderId, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteFolderUser([FromServices] OrganizationFolderUserService service, uint organizationId, uint folderId, uint userId)
        {
            var res = await service.DeleteFolderUser(organizationId, folderId, userId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
