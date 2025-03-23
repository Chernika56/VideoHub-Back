namespace BackEnd.DB.Entities
{
    public class M2mUsersCamerasEntity
    {
        public uint Id { get; set; }

        public uint UserId { get; set; }

        public uint CameraId { get; set; }

        public bool Favorite { get; set; }

        public bool MotionAlarm { get; set; }

        public virtual UsersEntity User { get; set; } = null!;

        public virtual CamerasEntity Camera { get; set; } = null!;
    }
}
