using BackEnd.Models;

namespace BackEnd.DTO.ResponseTO
{
    public class CameraResponseTO
    {
        public uint Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public string? Type { get; set; }

        public int? ArchiveTime { get; set; }

        public string? Ip { get; set; }
    }
}
