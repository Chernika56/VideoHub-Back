namespace BackEnd.Users.DTO.RequestDTO
{
    public class UserRequestDTO
    {
        public string? Login { get; set; } = null!;

        // public string? Password { get; set; } = null!;

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Note { get; set; }

        public uint? MaxSessions { get; set; }

        public bool? Disabled { get; set; }

        public string? AccessLevel { get; set; }

        public List<Organizations>? Organizations { get; set; } = new List<Organizations>();

        public List<Folders>? Folders { get; set; } = new List<Folders>();
    }
}
