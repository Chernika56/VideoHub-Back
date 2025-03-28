﻿namespace BackEnd.DB.Entities
{
    public class FoldersEntity
    {
        public uint Id { get; set; }

        public uint OrganizationId { get; set; }
        public virtual OrganizationsEntity Organization { get; set; } = null!;

        public uint? ParentId { get; set; }
        public virtual FoldersEntity? ParentFolder { get; set; }

        public string Title { get; set; } = null!;

        public uint CameraCount { get; set; }

        // hierarchy
        public uint HierarchyLevel { get; set; }

        // public uint? hierarchy_OrderNum { get; set; }  // ??????????
        // hierarchy

        // coordinates
        public float? CoordinatesLatitude { get; set; }

        public float? CoordinatesLongitude { get; set; }
        // coordinates

        public virtual ICollection<FoldersEntity>? ChildFolders { get; set; } = new List<FoldersEntity>();

        public virtual ICollection<CamerasEntity>? Cameras { get; set; } = new List<CamerasEntity>();

        public virtual ICollection<M2mUsersFoldersEntity>? M2mUsersFolders { get; set; } = new List<M2mUsersFoldersEntity>();

        //// floor_plan
        //// file
        //public string? file_Url { get; set; }
        //// file

        //// bottomleft
        //public float? bottomleft_Latitude { get; set; }

        //public float? bottomleft_Longitude { get; set; }
        //// bottomleft

        //// topleft
        //public float? topleft_Latitude { get; set; }

        //public float? topleft_Longitude { get; set; }
        //// topleft

        //// topright
        //public float? topright_Latitude { get; set; }

        //public float? topright_Longitude { get; set; }
        //// topright
    }
}
