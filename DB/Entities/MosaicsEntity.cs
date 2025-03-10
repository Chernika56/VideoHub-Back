namespace BackEnd.DB.Entities
{
    public class MosaicsEntity
    {
        public uint Id { get; set; }

        public uint OrganizationId { get; set; }

        public OrganizationsEntity Organization { get; set; } = null!;

        public ICollection<M2mMosaicsCamerasEntity> M2mMosaicsCameras { get; set; } = new List<M2mMosaicsCamerasEntity>();


        public string? Title { get; set; }

        public string? Type { get; set; }

        public bool? Visible { get; set; }
    }
}
