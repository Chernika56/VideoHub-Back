using BackEnd.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.DTO;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CameraController(ICameraService cameraService, ILogger<CameraController> logger) : Controller
    {
        [HttpGet]
        public async Task<IResult> GetAllCameras()
        {
            var result = await cameraService.GetAllCameras();

            if (result != null)
            {
                logger.LogInformation($"Cameras were getted succesfuly");

                var response = new MSRespone(StatusCodes.Status302Found, "Cameras were geted", result);
                Response.StatusCode = StatusCodes.Status302Found;
                return TypedResults.Json(response);
            }
            else
            {

                logger.LogError("Error during cameras getting");

                var responce = new MSRespone(StatusCodes.Status400BadRequest, "Error during cameras getting", result);
                Response.StatusCode = StatusCodes.Status400BadRequest;

                return TypedResults.Json(responce);
            }
        }
    }
}
