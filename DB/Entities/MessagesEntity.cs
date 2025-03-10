namespace BackEnd.DB.Entities
{
    public class MessagesEntity
    {
        public uint Id { get; set; }


        public string? Title { get; set; }

        public string? Body { get; set; }

        public string? Type { get; set; }

        public bool? IsPush { get; set; }

        public bool? IsDashboard { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? WasRead { get; set; }

        public uint SenderId { get; set; }

        public uint UserId { get; set; }

        public virtual UsersEntity User { get; set; } = null!;
    }
}
