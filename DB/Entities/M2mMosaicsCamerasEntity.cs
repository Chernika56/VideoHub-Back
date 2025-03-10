namespace BackEnd.DB.Entities
{
    public class M2mMosaicsCamerasEntity
    {
        public uint Id { get; set; }

        public uint MosaicId { get; set; }

        public uint CameraId { get; set; }

        public MosaicsEntity Mosaic { get; set; } = null!;

        public CamerasEntity Camera { get; set; } = null!;
    }
}
