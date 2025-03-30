using BackEnd.Streamers.Services;
using BackEnd.Users.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Streamers.Endpoints
{
    public static class StremerEndoints
    {
        public static void MapStreamers(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetStreamers)
                .WithOpenApi();
            builder.MapGet("/{streamerId:int}", GetStreamer)
                .WithOpenApi();
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private static async Task<IResult> GetStreamers([FromServices] StreamerService service)
        {
            var streamers = await service.GetStreamers();

            return streamers is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(streamers);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> GetStreamer([FromServices] StreamerService service, int streamerId)
        {
            var streamer = await service.GetStreamer(streamerId);

            return streamer is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(streamer);
        }
    }
}
