namespace BackEnd.DB.Entities
{
    public class PresetsEntity
    {
        public uint Id { get; set; }

        public string Title { get; set; } = null!;

        public ICollection<CamerasEntity>? Cameras { get; set; } = new List<CamerasEntity>();


        public float? DVRDepth { get; set; }

        public uint? DVRLockDays { get; set; }

        public uint? DVRSpace { get; set; }

        public bool IsAdjustable { get; set; }
        
        public bool IsDefault { get; set; }

        public uint PreciseTrumbnailsDays { get; set; }


        public bool IsDeleted { get; set; }


        //public string? VisionAlg { get; set; }

        //public string? VisionAreas { get; set; }

        //public bool? VisionEbabled { get; set; }

        //public string? VisionGPU { get; set; }

        //public string? ViseionParams { get; set; }

        //public string? Transcoder { get; set; }

    }
}
