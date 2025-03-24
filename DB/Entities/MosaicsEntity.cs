namespace BackEnd.DB.Entities
{
    public class MosaicsEntity
    {
        public uint Id { get; set; }

        public uint OrganizationId { get; set; }
        public virtual OrganizationsEntity Organization { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Type { get; set; } = null!;

        public bool Visible { get; set; }

        public virtual ICollection<M2mMosaicsCamerasEntity> M2mMosaicsCameras { get; set; } = new List<M2mMosaicsCamerasEntity>();
    }
}
