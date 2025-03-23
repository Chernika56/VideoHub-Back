namespace BackEnd.Users.DTO.RequestDTO
{
    public class UserFolderRequestDTO
    {
        public uint FolderId { get; set; }

        public uint UserId { get; set; }

        public bool CanView { get; set; }
    }
}
