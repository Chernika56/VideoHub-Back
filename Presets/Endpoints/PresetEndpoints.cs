using BackEnd.Cameras.DTO.RequestDTO;
using BackEnd.Cameras.Services;
using BackEnd.Presets.DTO.RequestDTO;
using BackEnd.Presets.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Presets.Endpoints
{
    public static class PresetEndpoints
    {
        public static void MapPresets(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetPresets)
                .WithOpenApi();
            builder.MapGet("/{presetId:uint}", GetPreset)
                .WithOpenApi();
            builder.MapPost("/", CreatePresets)
                .WithOpenApi();
            builder.MapPut("/{presetId:uint}", ChangePreset)
                .WithOpenApi();
            builder.MapDelete("/{presetId:uint}", DeletePreset)
                .WithOpenApi();
        }

        [Authorize]
        private static async Task<IResult> GetPresets([FromServices] PresetService service)
        {
            var presets = await service.GetPresets();

            return presets is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(presets);
        }

        [Authorize]
        private static async Task<IResult> GetPreset([FromServices] PresetService service, uint presetId)
        {
            var preset = await service.GetPreset(presetId);

            return preset is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(preset);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> CreatePresets([FromServices] PresetService service, [FromBody] PresetRequestDTO dto, uint presetId)
        {
            var preset = await service.CreatePresets(dto);

            return preset is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(preset);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangePreset([FromServices] PresetService service, [FromBody] PresetRequestDTO dto, uint presetId)
        {
            var preset = await service.ChangePreset(dto, presetId);

            return preset is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(preset);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeletePreset([FromServices] PresetService service, uint presetId)
        {
            var res = await service.DeletePreset(presetId);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
