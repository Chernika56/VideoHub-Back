using BackEnd.Folders.Services;
using BackEnd.Organizations.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Folders.Endpoints
{
    public static class FolderEndpoints
    {
        public static void MapFolders(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetFolders)
                .WithOpenApi();
            builder.MapGet("/{folderId:int}", GetFolder)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> GetFolders([FromServices] FolderService service)
        {
            var folders = await service.GetFolders();

            return folders is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folders);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> GetFolder([FromServices] FolderService service, int folderId)
        {
            var folder = await service.GetFolder(folderId);

            return folder is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(folder);
        }
    }
}
