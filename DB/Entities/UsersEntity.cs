using BackEnd.DB.Entities;

namespace BackEnd.DB.Entities
{
    public class UsersEntity
    {
        public uint Id { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Name { get; set; } 

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Note { get; set; }

        public string AccessLevel { get; set; } = null!;

        public uint? MaxSessions { get; set; }

        public bool Disabled { get; set; }

        public bool IsLoggedIn { get; set; }

        //playback_config
        public string Token { get; set; } = null!;
        //playback_config

        public virtual ICollection<M2mUsersOrganizationsEntity>? M2mUsersOrganizations { get; set; } = new List<M2mUsersOrganizationsEntity>();

        public virtual ICollection<M2mUsersFoldersEntity>? M2mUsersFolders { get; set; } = new List<M2mUsersFoldersEntity>();

        public virtual ICollection<M2mUsersCamerasEntity>? M2mUsersCameras{ get; set; } = new List<M2mUsersCamerasEntity>();

        public virtual ICollection<MessagesEntity>? SendMessages { get; set; } = new List<MessagesEntity>();

        public virtual ICollection<MessagesEntity>? ReceivedMessages { get; set; } = new List<MessagesEntity>();

        // public string? ApiKey { get; set; } // ????
        // public string? Session { get; set; } // ????
        //public bool Readonly { get; set; } // default = false
        //public bool IsAdmin { get; set; } // default = false
        //public bool CanViewOrganizations { get; set; } // default = false
        //public bool CanEditOrganizations { get; set; } // default = false
        //public bool IsOrganizationsAdmin { get; set; } // default = false
        // Settings???
    }
}
