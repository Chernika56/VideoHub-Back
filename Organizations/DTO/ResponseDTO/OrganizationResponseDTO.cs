using BackEnd.DB.Entities;

namespace BackEnd.Organizations.DTO.ResponseDTO
{
    public class OrganizationResponseDTO
    {
        public uint Id { get; set; }

        public string Title { get; set; } = null!;

        public Stats Stats { get; set; } = null!;

        public Limits Limits { get; set; } = null!;

        public bool IsDefault { get; set; }

        public uint OwnerId { get; set; }
    }

    public class Stats
    {
        public uint UserCount { get; set; } // ???  сделать автоматичекий подсчет, если нужно

        public uint CameraCount { get; set; } // ???  сделать автоматичекий подсчет, если нужно

        public uint MosaicCount { get; set; } // ??? сделать автоматичекий подсчет, если нужно
    }

    public class Limits
    {
        public uint? UserLimit { get; set; }

        public uint? CameraLimit { get; set; }
    }
}
