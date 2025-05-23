﻿namespace BackEnd.DB.Entities
{
    public class OrganizationsEntity
    {
        public uint Id { get; set; }

        public string Title { get; set; } = null!;

        // stats
        public uint UserCount { get; set; }

        public uint CameraCount { get; set; } 

        public uint MosaicCount { get; set; } 
        // stats

        // limits
        public uint? UserLimit { get; set; }

        public uint? CameraLimit { get; set; }
        // limits

        public bool IsDefault { get; set; }

        public uint OwnerId { get; set; }

        public virtual ICollection<M2mUsersOrganizationsEntity>? M2mUsersOrganizations { get; set; } = new List<M2mUsersOrganizationsEntity>();

        public virtual ICollection<FoldersEntity>? Folders { get; set; } = new List<FoldersEntity>();

        public virtual ICollection<MosaicsEntity>? Mosaics { get; set; } = new List<MosaicsEntity>();
    }
}
