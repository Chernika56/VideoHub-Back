namespace BackEnd.Organizations.DTO.ResponseDTO
{
    public class FolderUserResponseDTO
    {
        public uint FolderId { get; set; }

        public uint UserId { get; set; }

        public bool CanView { get; set; }
    }
}
