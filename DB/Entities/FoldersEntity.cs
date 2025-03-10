using BackEnd.DB.Entities;

namespace BackEnd.DB.Entities
{
    public class FoldersEntity
    {
        public uint Id { get; set; }

        public uint OrganizationId { get; set; }

        public uint? ParentsId { get; set; }

        public virtual OrganizationsEntity Organization { get; set; } = null!;

        public virtual ICollection<CamerasEntity>? Cameras { get; set; } = new List<CamerasEntity>();

        public virtual FoldersEntity? ParentsFolder { get; set; } = null!;

        public virtual ICollection<FoldersEntity>? ChildFolders { get; set; } = new List<FoldersEntity>();

        public virtual ICollection<M2mUsersFoldersEntity>? M2mUsersFolders { get; set; } = new List<M2mUsersFoldersEntity>();


        public uint CameraCount { get; set; } // ???  сделать автоматичекий подсчет, если нужно

        public string Title { get; set; } = null!;


        // hierarchy
        public uint hierarchy_Level { get; set; }

        public uint? hierarchy_OrderNum { get; set; }  // ??????????
        // hierarchy
        

        // coordinates
        public float? coordinates_Latitude { get; set; }

        public float? coordinates_Longitude { get; set; }
        // coordinates


        // floor_plan
        // file
        public string? file_Url { get; set; }
        // file

        // bottomleft
        public float? bottomleft_Latitude { get; set; }

        public float? bottomleft_Longitude { get; set; }
        // bottomleft

        // topleft
        public float? topleft_Latitude { get; set; }

        public float? topleft_Longitude { get; set; }
        // topleft

        // topright
        public float? topright_Latitude { get; set; }

        public float? topright_Longitude { get; set; }
        // topright
    }
}
