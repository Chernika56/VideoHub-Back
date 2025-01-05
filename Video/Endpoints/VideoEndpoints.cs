using BackEnd.Video.DTO;
using BackEnd.Video.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Video.Endpoints
{
    public static class VideoEndpoints
    {
        public static void MapVideo(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetVideo)
                .WithOpenApi();
            builder.MapGet("/list", GetVideoList)
                .WithOpenApi();
        }

        [Authorize]
        private static async Task<IResult> GetVideo(HttpContext context, [FromServices] VideoService service, [FromQuery] string path)
        {
            var fileInfo = await service.GetVideoFile(path, context);

            if (fileInfo.Value.file is null) return Results.NoContent();
            if (!fileInfo.Value.isPartitial) return Results.File(fileInfo.Value.file, "video/mp4");
            return Results.File(fileInfo.Value.file, "application/octet-stream", enableRangeProcessing: true);
        }

        [Authorize]
        private static IResult GetVideoList([FromServices] VideoService service)
        {
            var videoList = service.GetVideoList();

            return videoList is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(videoList);
        }
    }
}
