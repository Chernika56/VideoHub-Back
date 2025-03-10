namespace BackEnd.DB.Entities
{
    public class StreamersEntity
    {
        public uint Id { get; set; }

        public ICollection<CamerasEntity>? Cameras { get; set; } = new List<CamerasEntity>();


        public uint Port { get; set; }

        public string? Auth { get; set; }

        public bool? IsFilling { get; set; }

        public bool? HasAnalytics { get; set; }

        public bool? IsLocal { get; set; }

        // server_status наверное я должен получать непосредственно с сервера

        public string? ApiUrl { get; set; } 

        public string? PublicUrl { get; set; }

        public string? DVRPath { get; set; }

        public string? Hostname { get; set; }

        public bool? HasDVRStorage { get; set; }

        public string? ClusterKey { get; set; }

        public uint? MaxBitrate { get; set; }

        public bool? IsBackup { get; set; } 
    }
}
