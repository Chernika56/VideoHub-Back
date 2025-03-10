namespace BackEnd.DB.Entities
{
    public class M2mUsersFoldersEntity
    {
        public uint Id { get; set; }

        public uint UserId { get; set; }

        public uint FolderId { get; set; }

        public virtual UsersEntity User { get; set; } = null!;

        public virtual FoldersEntity Folder { get; set; } = null!;


        // permissions
        public bool CanView { get; set; } // default = false

        public bool CanViewDVR { get; set; } // default = false

        public uint? DVRDepthLimit { get; set; } 

        public bool? CanUsePTZ { get; set; } // default = false

        public bool? CanUseActions { get; set; } // default = false
        // permissions
    }
}
