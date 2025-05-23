﻿namespace BackEnd.DB.Entities
{
    public class M2mUsersFoldersEntity
    {
        public uint UserId { get; set; }
        public virtual UsersEntity User { get; set; } = null!;

        public uint FolderId { get; set; }
        public virtual FoldersEntity Folder { get; set; } = null!;

        public bool CanView { get; set; }

        //// permissions
        //public bool CanView { get; set; } // default = false

        //public bool CanViewDVR { get; set; } // default = false

        //public uint? DVRDepthLimit { get; set; } 

        //public bool? CanUsePTZ { get; set; } // default = false

        //public bool? CanUseActions { get; set; } // default = false
        //// permissions
    }
}
