using BackEnd.Folders.DTO.ResponseDTO;

namespace BackEnd.Users.DTO.ResponseDTO
{
    public class MyFoldersResponseDTO : FolderResponseDTO
    {
        public bool CanView { get; set; }
    }
}
