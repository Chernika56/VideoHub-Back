namespace BackEnd.DB.Entities
{
    public class M2mUsersCamerasEntity
    {
        public uint UserId { get; set; }
        public virtual UsersEntity User { get; set; } = null!;

        public string CameraName { get; set; } = null!;
        public virtual CamerasEntity Camera { get; set; } = null!;

        public bool Favorite { get; set; }

        public bool MotionAlarm { get; set; }
    }
}
