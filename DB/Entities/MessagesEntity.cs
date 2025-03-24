namespace BackEnd.DB.Entities
{
    public class MessagesEntity
    {
        public uint Id { get; set; }

        public uint UserId { get; set; }
        public virtual UsersEntity User { get; set; } = null!;

        public uint SenderId { get; set; }
        public virtual UsersEntity Sender { get; set; } = null!;

        public string? Title { get; set; }

        public string Body { get; set; } = null!;

        public string? Type { get; set; }

        public bool IsPush { get; set; }

        public bool IsDashboard { get; set; }

        public bool IsDeleted { get; set; }

        public bool WasRead { get; set; }
    }
}
