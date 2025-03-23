namespace BackEnd.Users.DTO.RequestDTO
{
    public class CreateUserDTO
    {
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Note { get; set; }

        public uint? MaxSession { get; set; }

        public bool Disabled { get; set; } = false;

        public string AccessLevel { get; set; } = "User";

        public List<Organizations>? Organizations { get; set; } = new List<Organizations>();

        public List<Folders>? Folders { get; set; } = new List<Folders>();
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
