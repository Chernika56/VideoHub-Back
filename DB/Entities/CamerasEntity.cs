using Microsoft.CodeAnalysis;

namespace BackEnd.DB.Entities
{
    public class CamerasEntity
    {
        public string Name { get; set; } = null!; // сделать первичным ключом

        public uint FolderId { get; set; }
        public virtual FoldersEntity Folder { get; set; } = null!;

        public uint PresetId { get; set; }
        public virtual PresetsEntity Preset { get; set; } = null!;

        public uint StreamerId { get; set; }
        public virtual StreamersEntity Streamer { get; set; } = null!;

        public uint OrganizationId { get; set; }

        public string Comment { get; set; } = null!;

        public string? Coordinates { get; set; } = null!;

        public bool Enabled { get; set; }

        public float? DVRDepth { get; set; } // (архив) дни // первично из presets потом может изменится

        public float? DVRLockDays { get; set; } // (Лимит DVR в днях для записей по движению) дни // первично из presets потом может изменится

        public string? DVRPath { get; set; }

        public uint? DVRSpace { get; set; } // (Пространство DVR) ГБ // первично из presets потом может изменится

        public DateTimeOffset LastEventTime { get; set; }

        public bool? MotionDetectorEnabled { get; set; }

        public string? OnvifProfile { get; set; }

        public bool? OnvifPTZ { get; set; }

        public string? OnvifURL { get; set; }

        //permissions
        public bool? View { get; set; }

        public bool? Edit { get; set; }

        public bool? PTZ { get; set; }  

        public bool? DVR { get; set; }

        public uint DVRDepthLimit { get; set; }

        public bool? Actions { get; set; }
        //permissions

        public string? PostalAddress { get; set; }

        public string StreamUrl { get; set; } = null!;

        public string? SubStreamUrl { get; set; }

        public string Title { get; set; } = null!;

        public virtual ICollection<EventsEntity>? Events { get; set; } = new List<EventsEntity>();

        public virtual ICollection<M2mMosaicsCamerasEntity>? M2mMosaicsCameras { get; set; } = new List<M2mMosaicsCamerasEntity>();

        public virtual ICollection<M2mUsersCamerasEntity> M2MUsersCameras { get; set; } = new List<M2mUsersCamerasEntity>();

        //playback_config
        //public string Token { get; set; } = null!;
        //playback_config

        // public uint PreciseThumbnailsDays { get; set; }

        // public string? Server { get; set; }

        // public bool? Static { get; set; }

        ////stream_status
        //public string Stream_Name { get; set; } = null!;

        //public string? Stream_Server { get; set; }

        //public bool? Alive { get; set; }

        //public uint? Lifetime { get; set; }

        //public uint Bitrate { get; set; }

        //public uint SourceError { get; set; }

        //public uint HttpPort { get; set; }

        //public uint HttpsPort { get; set; }
        ////stream_status

        //public bool? Thumbnails { get; set; }

        //public string? ThumbnailsUrl { get; set; }

        ////user_attributes
        //public bool? Favorite { get; set; }

        //public bool? MotionAlarm { get; set; }
        ////user_attributes

        // public bool? VideoOnly { get; set; }
    }
}
