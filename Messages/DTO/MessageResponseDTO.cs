namespace BackEnd.Messages.DTO
{
    public class MessageResponseDTO
    {
        public uint Id { get; set; }


        public string? Title { get; set; }

        public string Body { get; set; } = null!;

        public string? Type { get; set; }

        public bool? IsPush { get; set; }

        public bool? IsDashboard { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? WasRead { get; set; }

        public User Sender { get; set; } = null!;

        public User User { get; set; } = null!;
    }

    public class User
    {
        public uint Id { get; set; }

        public string Login { get; set; } = null!;
    }
}
