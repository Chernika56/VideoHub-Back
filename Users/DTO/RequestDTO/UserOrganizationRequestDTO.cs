namespace BackEnd.Users.DTO.RequestDTO
{
    public class UserOrganizationRequestDTO
    {
        public uint OrganizationId { get; set; }

        public uint UserId { get; set; }

        public Permissions UserPermissions { get; set; } = null!;
    }

    public class Permissions
    {
        public bool IsMember { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}
