using BackEnd.Folders.DTO.ResponseDTO;

namespace BackEnd.Folders.DTO.RequestDTO
{
    public class FolderRequestDTO
    {
        // public uint Id { get; set; }

        public uint? OrganizationId { get; set; }

        public uint? ParentId { get; set; }

        // public uint CameraCount { get; set; } // ???  сделать автоматичекий подсчет, если нужно

        public string? Title { get; set; } = null!;

        public Hierarchy? Hierarchy { get; set; } = null!;

        public Coordinates? Coordinates { get; set; } = null!;

        // public FloorPlan? FloorPlan { get; set; } = null!;
    }

    public class Hierarchy
    {
        public uint? Level { get; set; }

        // public uint? OrderNum { get; set; }
    }

    public class Coordinates
    {
        public float? Latitude { get; set; }

        public float? Longitude { get; set; }
    }

    //public class FloorPlan
    //{
    //    public File? File { get; set; } = null!;

    //    public Coordinates? BottomLeft { get; set; } = null!;

    //    public Coordinates? TopLeft { get; set; } = null!;

    //    public Coordinates? TopRight { get; set; } = null!;
    //}

    //public class File
    //{
    //    public string? Url { get; set; }
    //}
}
