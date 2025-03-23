using BackEnd.Cameras.DTO.RequestDTO;
using BackEnd.Cameras.Services;
using BackEnd.Utils.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Cameras.Endpoints
{
    public static class CameraEndpoints
    {
        public static void MapCameras(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/", GetCameras)
                .WithOpenApi();
            builder.MapGet("/{cameraName:string}", GetCamera)
                .WithOpenApi();
            builder.MapPut("/{cameraName:string}", ChangeCamera)
                .WithOpenApi();
            builder.MapDelete("/{cameraName:string}", DeleteCamera)
                .WithOpenApi();
        }

        [Authorize]
        private static async Task<IResult> GetCameras([FromServices] CameraService service, [FromQuery] string? folder_id)
        {
            var cameras = await service.GetCameras(folder_id);

            return cameras is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(cameras);
        }

        [Authorize]
        private static async Task<IResult> GetCamera([FromServices] CameraService service, string cameraName)
        {
            var camera = await service.GetCamera(cameraName);

            return camera is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(camera);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> ChangeCamera([FromServices] CameraService service, [FromBody] CameraRequestDTO dto, string cameraName)
        {
            var camera = await service.ChangeCamera(dto, cameraName);

            return camera is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.Ok(camera);
        }

        [Authorize(Policy = PolicyType.OrganizationAdminPolicy)]
        private static async Task<IResult> DeleteCamera([FromServices] CameraService service, string cameraName)
        {
            var res = await service.DeleteCamera(cameraName);

            return res is null ? Results.StatusCode(StatusCodes.Status500InternalServerError) : Results.StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
