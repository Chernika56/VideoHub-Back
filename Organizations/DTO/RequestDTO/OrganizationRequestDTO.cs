using BackEnd.Organizations.DTO.ResponseDTO;

namespace BackEnd.Organizations.DTO.RequestDTO
{
    public class OrganizationRequestDTO
    {
        // public uint Id { get; set; }

        public string? Title { get; set; } = null!;

        // public Stats Stats { get; set; } = null!;

        public Limits? Limits { get; set; } = null!;

        public bool? IsDefault { get; set; }

        public uint? OwnerId { get; set; }
    }

    public class Limits
    {
        public uint? UserLimit { get; set; }

        public uint? CameraLimit { get; set; }
    }
}
