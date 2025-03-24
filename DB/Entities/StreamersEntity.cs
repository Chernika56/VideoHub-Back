using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackEnd.DB.Entities
{
    public class StreamersEntity
    {
        public uint Id { get; set; }

        public string HostName { get; set; } = null!;

        public string ApiUrl { get; set; } = null!;

        public uint Port { get; set; }

        public bool IsLocal { get; set; }

        public string DVRPath { get; set; } = null!;

        public virtual ICollection<CamerasEntity>? Cameras { get; set; } = new List<CamerasEntity>();


        //public string? Auth { get; set; }

        //public bool? IsFilling { get; set; }

        //public bool? HasAnalytics { get; set; }



        // server_status наверное я должен получать непосредственно с сервера



        //public string? PublicUrl { get; set; }

        //public bool? HasDVRStorage { get; set; }

        //public string? ClusterKey { get; set; }

        //public uint? MaxBitrate { get; set; }

        //public bool? IsBackup { get; set; } 
    }
}
