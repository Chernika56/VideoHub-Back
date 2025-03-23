using BackEnd.Authorization.Services;
using BackEnd.DB.Context;
using BackEnd.Users.Services;

namespace BackEnd.Messages.DTO
{
    public class MessageSendDTO
    {
        public string? Title { get; set; }

        public string Body { get; set; } = null!;

        public string? Type { get; set; }

        public bool? IsPush { get; set; }

        public bool? IsDashboard { get; set; }

        public uint UserId { get; set; }
    }
}
