namespace BackEnd.DB.Entities
{
    public class M2mUsersOrganizationsEntity
    {
        public uint Id { get; set; }

        public uint UserId { get; set; }

        public uint OrganizationId { get; set; }

        public virtual UsersEntity User { get; set; } = null!;

        public virtual OrganizationsEntity Organization { get; set; } = null!;


        // permissions
        public bool IsMember { get; set; } // default = false

        public bool CanViewStats { get; set; } // default = false

        public bool CanEditCameras { get; set; } // default = false

        public bool CanEditUsers { get; set; } // default = false

        public bool CanViewPersonList { get; set; } // default = false

        public bool CanEditPersonList { get; set; } // default = false
        // permissions

    }
}
