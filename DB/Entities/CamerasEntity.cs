using Microsoft.CodeAnalysis;

namespace BackEnd.DB.Entities
{
    public class CamerasEntity
    {
        public uint Id { get; set; }

        public uint FolderId { get; set; }
        // folder_coordinates

        public uint PresetId { get; set; }
        // all

        public uint AgentId { get; set; }
        // agent_id, agent_key, agent_model, agent_serial, agent_status

        public uint StreamerId { get; set; }
        // public string? DVRPath { get; set; } // из streamers

        public FoldersEntity Folder { get; set; } = null!;

        public PresetsEntity Preset { get; set; } = null!;

        public AgentsEntity Agent { get; set; } = null!;

        public StreamersEntity Streamer { get; set; } = null!;

        public ICollection<EventsEntity>? Events { get; set; } = new List<EventsEntity>();

        public ICollection<M2mMosaicsCamerasEntity>? M2mMosaicsCameras { get; set; } = new List<M2mMosaicsCamerasEntity>();


        public string Name { get; set; } = null!;

        public string Comment { get; set; } = "";

        public string? Coordinates { get; set; } = "";

        public bool Enabled { get; set; }

        public float? DVRDepth { get; set; } // (архив) дни // первично из presets потом может изменится

        public float? DVRLockDays { get; set; } // (Лимит DVR в днях для записей по движению) дни // первично из presets потом может изменится

        public uint? DVRSpace { get; set; } // (Пространство DVR) ГБ // первично из presets потом может изменится

        public bool? HasActions { get; set; }

        // lastChange???

        public DateTimeOffset LastEventTime { get; set; }

        public bool? MotionDetectorEnabled { get; set; }

        public string? OnvifProfile { get; set; }

        public bool? OnvifRTZ { get; set; }

        public string? OnvifURL { get; set; }


        //permissions
        public bool? View { get; set; }

        public bool? Edit { get; set; }

        public bool? PTZ { get; set; }  

        public bool? DVR { get; set; }

        public uint DVRDepthLimit { get; set; }

        public bool? Actions { get; set; }
        //permissions

        //playback_config
        public string Token { get; set; } = null!;
        //playback_config

        public string? PostalAddress { get; set; }

        public uint PreciseThumbnailsDays { get; set; }

        public string? Server { get; set; }

        public bool? Static { get; set; }

        //stream_status
        public string Stream_Name { get; set; } = null!;

        public string? Stream_Server { get; set; }

        public bool? Alive { get; set; }

        public uint? Lifetime { get; set; }

        public uint Bitrate { get; set; }

        public uint SourceError { get; set; }

        public uint HttpPort { get; set; }

        public uint HttpsPort { get; set; }
        //stream_status

        public string StreamUrl { get; set; } = null!;

        public string? SubStreamUtl { get; set; }

        public bool? Thumbnails { get; set; }

        public string? ThumbnailsUrl { get; set; }

        public string Title { get; set; } = null!;

        //user_attributes
        public bool? Favorite { get; set; }

        public bool? MotionAlarm { get; set; }
        //user_attributes

        public bool? VideoOnly { get; set; }
    }
}
