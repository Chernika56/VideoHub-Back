namespace BackEnd.Mosaics.DTO
{
    public class MosaicsResponseDTO
    {
        public uint Id { get; set; }

        public uint OrganizationId { get; set; }

        public string Title { get; set; } = null!;

        public string Type { get; set; } = null!;

        public bool Visible { get; set; }
    }
}
