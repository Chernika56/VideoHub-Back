namespace BackEnd.DB.Entities
{
    public class OrganizationsEntity
    {
        public uint Id { get; set; }

        public string Title { get; set; } = null!;

        public virtual ICollection<M2mUsersOrganizationsEntity>? M2mUsersOrganizations { get; set; } = new List<M2mUsersOrganizationsEntity>();

        public virtual ICollection<FoldersEntity>? Folders { get; set; } = new List<FoldersEntity>();

        public virtual ICollection<MosaicsEntity>? Mosaics { get; set; } = new List<MosaicsEntity>();


        public virtual ICollection<CamerasEntity> Cameras { get; set; } = new List<CamerasEntity>();


        // нужно сделать триггеры, которые будут обновлять, или если я управляю бд только через сервер, то сделать это на строне сервера
        // stats
        public uint UserCount { get; set; } // сделать триггер при добавлении записи в M2mUsersOrganizations

        public uint CameraCount { get; set; } // сделать триггер при добавлении записи в Cameras 

        public uint MosaicCount { get; set; } // сделать триггер при добавлении записи в Mosaics
        // stats


        // limits
        public uint? UserLimit { get; set; }

        public uint? CameraLimit { get; set; }
        // limits

        public bool IsDefault { get; set; } // falault = true

        public uint OwnerId { get; set; }
    }
}
