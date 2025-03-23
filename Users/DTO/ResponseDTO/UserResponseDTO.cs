using BackEnd.DB.Entities;
using BackEnd.Users.DTO.RequestDTO;

namespace BackEnd.Users.DTO.ResponseDTO
{
    public class UserResponseDTO
    {
        public uint Id { get; set; }

        public string Login { get; set; } = null!;

        public string? Name { get; set; }

        // public string Password { get; set; } = null!;

        public string? Email { get; set; }

        // public string? ApiKey { get; set; } // ????

        // public string? Session { get; set; } // ????


        public string? Phone { get; set; }

        public string? Note { get; set; }

        public uint? MaxSessions { get; set; }

        public bool Disabled { get; set; }

        public string AccessLevel { get; set; } = null!;

        public bool IsLoggedIn { get; set; } // default = false

        public List<Organizations>? Organizations { get; set; } = new List<Organizations>();

        public List<Folders>? Folders { get; set; } = new List<Folders>();
        
        // Settings???
    }

    public class Organizations
    {
        public uint Id { get; set; }

        public bool IsMember { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class Folders
    {
        public uint Id { get; set; }

        public bool CanView { get; set; }
    }
}
