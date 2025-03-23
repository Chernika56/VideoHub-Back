namespace BackEnd.Users.DTO.ResponseDTO
{
    public class ProfileResponseDTO
    {
        public string Login { get; set; } = null!;

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Note { get; set; }

        public uint? MaxSession { get; set; }
    }
}
