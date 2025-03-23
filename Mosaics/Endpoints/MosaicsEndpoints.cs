using BackEnd.Mosaics.DTO;
using BackEnd.Mosaics.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Mosaics.Endpoints
{
    public static class MosaicsEndpoints
    {
        public static void MapMosaics(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetMosaics)
                .WithOpenApi();
            builder.MapGet("/{mosaicId:uint}", GetMosaic)
                .WithOpenApi();
            builder.MapPost("/", CreateMosaic)
                .WithOpenApi();
            builder.MapPut("/{mosaicId:uint}", ChangeMosaic)
                .WithOpenApi();
            builder.MapDelete("/{mosaicId:uint}", DeleteMosaic)
                .WithOpenApi();
        }

        [Authorize]
        private async static Task<IResult> GetMosaics([FromServices] MosaicsService service, [FromQuery] string? organizationId)
        {
            var mosaics = await service.GetMosaics(organizationId);

            return mosaics is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(mosaics);
        }

        [Authorize]
        private async static Task<IResult> GetMosaic([FromServices] MosaicsService service, uint mosaicId)
        {
            var mosaic = await service.GetMosaic(mosaicId);

            return mosaic is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(mosaic);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private async static Task<IResult> CreateMosaic([FromServices] MosaicsService service, MosaicCreateDTO dto)
        {
            var mosaic = await service.CreateMosaic(dto);

            return mosaic is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(mosaic);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private async static Task<IResult> ChangeMosaic([FromServices] MosaicsService service, MosaicRequestDTO dto, uint mosaicId)
        {
            var mosaic = await service.ChangeMosaic(dto, mosaicId);

            return mosaic is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(mosaic);
        }

        [Authorize(Policy = PolicyType.AdministratorPolicy)]
        private async static Task<IResult> DeleteMosaic([FromServices] MosaicsService service, uint mosaicId)
        {
            var res = await service.DeleteMosaic(mosaicId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
