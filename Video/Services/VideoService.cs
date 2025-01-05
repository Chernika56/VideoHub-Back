using BackEnd.Video.DTO;
using BackEnd.Video.Utils;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Video.Services
{
    public class VideoService(ILogger<VideoService> logger)
    {
        private const string videoFolderPath = "D:\\Diplom\\Videos\\";

        public async Task<(FileStream? file, bool isPartitial)?> GetVideoFile(string path, HttpContext context)
        {
            return await Task.Run(() =>
            {
                var response = context.Response;
                var request = context.Request;
                var filePath = Path.Combine(videoFolderPath, path);

                if (!File.Exists(filePath))
                    return (null!, false);

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileLength = fileStream.Length;
                var rangeHeader = request.Headers["Range"].ToString();


                if (string.IsNullOrEmpty(rangeHeader))
                {
                    return (fileStream, false);
                }

                var range = rangeHeader.Replace("bytes=", "").Split('-');
                var start = long.Parse(range[0]);
                var end = range.Length > 1 && !string.IsNullOrEmpty(range[1]) ? long.Parse(range[1]) : fileLength - 1;

                if (start >= fileLength || end >= fileLength)
                    return (null!, false);

                fileStream.Seek(start, SeekOrigin.Begin);

                var contentLength = end - start + 1;
                response.StatusCode = 206;
                response.Headers["Content-Range"] = $"bytes {start}-{end}/{fileLength}";
                response.Headers.ContentLength = contentLength;
                return (fileStream, true);
            });
        }

        public List<VideoResponseDTO>? GetVideoList()
        {
            try
            {
                var videos = VideoStorage.Videos.Select(v => new VideoResponseDTO()
                {
                    VideoName = v.VideoName,
                    VideoPath = v.VideoPath
                }).ToList();


                return videos;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during film finding {exception}", ex);
                return null;
            }
        }
    }
}
