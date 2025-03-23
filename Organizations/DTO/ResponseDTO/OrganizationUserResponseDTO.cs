namespace BackEnd.Organizations.DTO.ResponseDTO
{
    public class OrganizationUserResponseDTO
    {
        public uint Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public Permissions Permissions { get; set; } = null!;
    }

    public class Permissions
    {
        public ICollection<FolderPermissions>? Folders { get; set; } = new List<FolderPermissions>();

        public OrganizationPermissions Organization { get; set; } = null!;
    }

    public class FolderPermissions
    {
        public uint Id { get; set; }

        public bool CanView { get; set; }
    }

    public class OrganizationPermissions
    {
        public bool IsMember { get; set; }

        public bool IsAdmin { get; set; }
    }
}
