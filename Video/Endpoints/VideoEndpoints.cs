using BackEnd.Video.DTO;
using BackEnd.Video.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BackEnd.Video.Endpoints
{
    public static class VideoEndpoints
    {
        public static void MapVideo(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetVideo)
                .WithOpenApi();
            builder.MapGet("/playlist", GetVideoPlaylist)
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
        private static async Task<IResult> GetVideoPlaylist([FromServices] VideoService videoService, [FromQuery] string videoName)
        {
            string videoFolderPath = "D:\\Diplom\\Videos\\";
            // Пусть videoName — имя видео без фрагментации.
            var videoPath = Path.Combine(videoFolderPath, videoName);
            if (!Directory.Exists(videoPath))
                return Results.NotFound("Видео не найдено.");

            var playlistBuilder = new StringBuilder();
            playlistBuilder.AppendLine("#EXTM3U");
            playlistBuilder.AppendLine("#EXT-X-VERSION:3");
            playlistBuilder.AppendLine("#EXT-X-TARGETDURATION:60");
            playlistBuilder.AppendLine("#EXT-X-MEDIA-SEQUENCE:0");

            // Генерируем плейлист из всех фрагментов
            var fragments = Directory.GetFiles(videoPath, "*.mp4")
                                      .OrderBy(name => name); // Убедитесь, что порядок верен
            foreach (var fragment in fragments)
            {
                playlistBuilder.AppendLine("#EXTINF:60.0,");
                playlistBuilder.AppendLine($"https://localhost:7277/api/v1.0/video?path={Path.GetFileName(fragment)}");
            }
            playlistBuilder.AppendLine("#EXT-X-ENDLIST");

            return Results.Content(playlistBuilder.ToString(), "application/vnd.apple.mpegurl");
        }

        [Authorize]
        private static IResult GetVideoList([FromServices] VideoService service)
        {
            var videoList = service.GetVideoList();

            return videoList is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(videoList);
        }
    }
}
