using BackEnd.DB.Entities;

namespace BackEnd.DB.Entities
{
    public class UsersEntity
    {
        public uint Id { get; set; }

        public string Login { get; set; } = null!; // сделать просто уникальным или мб вторичным ключом или типо того

        public string? Name { get; set; } 

        public string Password { get; set; } = null!;

        public string? Email { get; set; }

        public virtual ICollection<M2mUsersOrganizationsEntity>? M2mUsersOrganizations { get; set; } = new List<M2mUsersOrganizationsEntity>();

        public virtual ICollection<M2mUsersFoldersEntity>? M2mUsersFolders { get; set; } = new List<M2mUsersFoldersEntity>();


        // public string? ApiKey { get; set; } // ????

        // public string? Session { get; set; } // ????

        //playback_config
        public string Token { get; set; } = null!;
        //playback_config

        public string? Phone { get; set; }

        public string? Note { get; set; }

        public uint? MaxSessions { get; set; }

        // admin
        //public bool Readonly { get; set; } // default = false

        public bool Disabled { get; set; } // default = false
        // admin

        public string AccessLevel { get; set; } = null!;


        //public bool IsAdmin { get; set; } // default = false

        public bool IsLoggedIn { get; set; } // default = false

        //public bool CanViewOrganizations { get; set; } // default = false

        //public bool CanEditOrganizations { get; set; } // default = false

        //public bool IsOrganizationsAdmin { get; set; } // default = false


        // Settings???

        public virtual ICollection<MessagesEntity>? Messages { get; set; } = new List<MessagesEntity>();
    }
}
