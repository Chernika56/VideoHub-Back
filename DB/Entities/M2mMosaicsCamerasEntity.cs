namespace BackEnd.DB.Entities
{
    public class M2mMosaicsCamerasEntity
    {
        public uint MosaicId { get; set; }
        public virtual MosaicsEntity Mosaic { get; set; } = null!;

        public string CameraName { get; set; } = null!;
        public virtual CamerasEntity Camera { get; set; } = null!;
    }
}
