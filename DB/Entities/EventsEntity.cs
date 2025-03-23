namespace BackEnd.DB.Entities
{
    public class EventsEntity
    {
        public uint Id { get; set; }

        public uint CameraId { get; set; }

        public virtual CamerasEntity Camera { get; set; } = null!;


        public string Source { get; set; } = null!;

        public string Type { get; set; } = null!;

        public bool HasPreview { get; set; }

        public DateTimeOffset StartAt { get; set; }

        public DateTimeOffset EndAt { get; set; }

        
        //public string SourceId { get; set; }

        //public string? ObjectClass { get; set; }

        //public uint? ObjectId { get; set; }

        //public string? ObjectAction { get; set; }
    }
}
