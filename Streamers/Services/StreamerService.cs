using BackEnd.Authorization.Services;
using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.Organizations.DTO.ResponseDTO;
using BackEnd.Streamers.DTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Streamers.Services
{
    public class StreamerService(ILogger<CameraService> logger, MyDbContext db)
    {
        public async Task<List<StreamerResponseDTO>?> GetStreamers()
        {
            try
            {
                var query = db.Streamers.AsQueryable();

                var streamers = await query.Select(s => new StreamerResponseDTO
                {
                    Id = s.Id,
                    HostName = s.HostName,
                    ApiUrl = s.ApiUrl,
                    Port = s.Port,
                    IsLocal = s.IsLocal,
                    DVRPath = s.DVRPath
                }).ToListAsync();

                return streamers;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during streamer finding {exception}", ex);
                return null;
            }
        }

        public async Task<StreamerResponseDTO?> GetStreamer(int streamerId)
        {
            try
            {
                var query = db.Streamers
                    .Where(s => s.Id == streamerId)
                    .AsQueryable();

                var streamer = await query.Select(s => new StreamerResponseDTO
                {
                    Id = s.Id,
                    HostName = s.HostName,
                    ApiUrl = s.ApiUrl,
                    Port = s.Port,
                    IsLocal = s.IsLocal,
                    DVRPath = s.DVRPath
                }).FirstOrDefaultAsync();

                return streamer;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during streamer finding {exception}", ex);
                return null;
            }
        }
    }
}
