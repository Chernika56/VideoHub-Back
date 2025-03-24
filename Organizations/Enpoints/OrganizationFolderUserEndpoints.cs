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
            builder.MapGet("/{organizationId:int}/folders/{folderId:int}/users", GetFolderUsers)
                .WithOpenApi();
            builder.MapGet("/{organizationId:int}/folders/{folderId:int}/users/{userId:int}", GetFolderUser)
                .WithOpenApi();
            builder.MapPut("/{organizationId:int}/folders/{folderId:int}/users/{userId:int}", ChangeFolderUser)
                .WithOpenApi();
            builder.MapDelete("/{organizationId:int}/folders/{folderId:int}/users/{userId:int}", DeleteFolderUser)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetFolderUsers([FromServices] OrganizationFolderUserService service, int organizationId, int folderId)
        {
            var users = await service.GetFolderUsers(organizationId, folderId);

            return users is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(users);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetFolderUser([FromServices] OrganizationFolderUserService service, int organizationId, int folderId, int userId)
        {
            var user = await service.GetFolderUser(organizationId, folderId, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeFolderUser([FromServices] OrganizationFolderUserService service, [FromBody] FolderUserRequestDTO dto, int organizationId, int folderId, int userId)
        {
            var user = await service.ChangeFolderUser(dto, organizationId, folderId, userId);

            return user is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(user);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteFolderUser([FromServices] OrganizationFolderUserService service, int organizationId, int folderId, int userId)
        {
            var res = await service.DeleteFolderUser(organizationId, folderId, userId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
