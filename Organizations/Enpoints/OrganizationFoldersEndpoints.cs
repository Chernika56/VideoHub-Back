using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Organizations.Enpoints
{
    public static class OrganizationFoldersEndpoints
    {
        public static void MapOrganizationFolders(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/{organizationId:uint}/folders", GetOrganizationFolders)
                .WithOpenApi();
            builder.MapPost("/{organizationId:uint}/folders", CreateOrganizationFolder)
                .WithOpenApi();
            builder.MapGet("/{organizationId:uint}/folders/{folderId:uint}", GetOrganizationFolder)
                .WithOpenApi();
            builder.MapPut("/{organizationId:uint}/folders/{folderId:uint}", ChangeOrganizationFolder)
                .WithOpenApi();
            builder.MapDelete("/{organizationId:uint}/folders/{folderId:uint}", DeleteFolder)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetOrganizationFolders([FromServices] OrganizationFoldersService service, uint organizationId)
        {
            var folders = await service.GetOrganizationFolders(organizationId);

            return folders is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folders);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> CreateOrganizationFolder([FromServices] OrganizationFoldersService service, [FromBody] OrganizationFolderRequestDTO dto, uint organizationId)
        {
            var folder = await service.CreateOrganizationFolder(dto, organizationId);

            return folder is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folder);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetOrganizationFolder([FromServices] OrganizationFoldersService service, uint organizationId, uint folderId)
        {
            var folder = await service.GetOrganizationFolder(organizationId, folderId);

            return folder is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folder);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeOrganizationFolder([FromServices] OrganizationFoldersService service, [FromBody] OrganizationFolderRequestDTO dto, uint organizationId, uint folderId)
        {
            var folder = await service.ChangeOrganizationFolder(dto, organizationId, folderId);

            return folder is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folder);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteFolder([FromServices] OrganizationFoldersService service, uint organizationId, uint folderId)
        {
            var res = await service.DeleteFolder(organizationId, folderId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
