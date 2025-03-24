namespace BackEnd.Mosaics.DTO
{
    public class MosaicRequestDTO
    {
        public string? Title { get; set; } = null!;

        public string? Type { get; set; } = null!;

        public uint? OrganizationId { get; set; }

        public bool? Visible { get; set; }

        public ICollection<string>? Cameras { get; set; } = new List<string>();
    }
}
