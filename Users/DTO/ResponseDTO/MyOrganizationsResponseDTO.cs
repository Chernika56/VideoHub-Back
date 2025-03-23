using BackEnd.Organizations.DTO.ResponseDTO;

namespace BackEnd.Users.DTO.ResponseDTO
{
    public class MyOrganizationsResponseDTO : OrganizationResponseDTO
    {
        public Permissions UserPermissions { get; set; } = null!;
    }

    public class Permissions
    {
        public bool IsMember { get; set; } = false;

        public bool IsAdmin { get; set; } = false;
    }
}
