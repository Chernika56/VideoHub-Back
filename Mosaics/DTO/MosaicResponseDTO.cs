using BackEnd.Cameras.DTO.ResponseDTO;

namespace BackEnd.Mosaics.DTO
{
    public class MosaicResponseDTO : MosaicsResponseDTO
    {
        public ICollection<CameraResponseDTO> Cameras { get; set; } = new List<CameraResponseDTO>();
    }
}
