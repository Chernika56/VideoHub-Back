﻿namespace BackEnd.Cameras.DTO.RequestDTO
{
    public class CameraRequestDTO
    {
        // public uint Id { get; set; }

        // public AgentsDTO? Agents { get; set; }

        public string Name { get; set; } = null!;

        public string? Comment { get; set; }

        public string? Coordinates { get; set; }

        public float? DVRDepth { get; set; } // (архив) дни // первично из presets потом может изменится

        public float? DVRLockDays { get; set; } // (Лимит DVR в днях для записей по движению) дни // первично из presets потом может изменится

        public uint? DVRSpace { get; set; } // (Пространство DVR) ГБ // первично из presets потом может изменится

        // public bool? Enabled { get; set; }

        // public string? FolderCoordinates { get; set; } // из folders

        public uint? FolderId { get; set; }

        // public bool? HasActions { get; set; }

        // public ChangeDTO LastChange { get; set; } = null!;

        // public long? LastEventTime { get; set; }

        public bool? MotionDetectorEnabled { get; set; }

        public string? OnvifProfile { get; set; }

        public bool? OnvifPTZ { get; set; }

        public string? OnvifURL { get; set; }

        // public uint OrganizationId { get; set; } // через folder

        public Permissions? Permissions { get; set; }

        // public PlaybackConfig PlaybackConfig { get; set; } = null!;

        public string? PostalAddress { get; set; }

        // public PresetsDTO Preset { get; set; } = null!;

        public uint? PresetId { get; set; }

        // public string? Server { get; set; }

        // public bool? Static { get; set; }

        // public StreamStatus? StreamStatus { get; set; } = null!;

        public string? StreamUrl { get; set; }

        public uint? StreamerId { get; set; }

        public string? SubStreamUrl { get; set; }

        // public bool? Thumbnails { get; set; }

        // public string? ThumbnailsUrl { get; set; }

        public string? Title { get; set; }

        // public UserAttributes? UserAttributes { get; set; } // сделать отдельный запрос

        // public bool? VideoOnly { get; set; }

        // public string? VisionAlg { get; set; }

        // public string? VisionAreas { get; set; }

        // public bool? VisionEbabled { get; set; }

        // public string? VisionGPU { get; set; }
    }

    public class Permissions
    {
        public bool? View { get; set; }

        public bool? Edit { get; set; }

        public bool? PTZ { get; set; }

        public bool? DVR { get; set; }

        public uint? DVRDepthLimit { get; set; }

        public bool? Actions { get; set; }
    }

    //public class StreamStatus
    //{
    //    public string? Stream_Name { get; set; } = null!;

    //    public string? Stream_Server { get; set; }

    //    public bool? Alive { get; set; }

    //    public uint? Lifetime { get; set; }

    //    public uint? Bitrate { get; set; }

    //    public uint? SourceError { get; set; }

    //    public uint? HttpPort { get; set; }

    //    public uint? HttpsPort { get; set; }
    //}

    public class UserAttributes
    {
        public bool? Favorite { get; set; }

        public bool? MotionAlarm { get; set; }
    }
}
