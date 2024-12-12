using BackEnd.DTO.ResponseTO;

namespace BackEnd.Services.Interface
{
    public interface ICameraService
    {
        public Task<CameraResponseTO[]> GetAllCameras(); 
    }
}
