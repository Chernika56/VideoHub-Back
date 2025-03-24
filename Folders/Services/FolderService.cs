using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.Folders.DTO.ResponseDTO;
using BackEnd.Organizations.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Folders.Services
{
    public class FolderService(ILogger<CameraService> logger, MyDbContext db)
    {
        public async Task<List<FolderResponseDTO>?> GetFolders()
        {
            try
            {
                var query = db.Folders.AsQueryable();

                var folders = await query.Select(f => new FolderResponseDTO
                {
                    Id = f.Id,
                    OrganizationId = f.OrganizationId,
                    ParentsId = f.ParentId,
                    CameraCount = f.CameraCount,
                    Title = f.Title,
                    Hierarchy = new Hierarchy
                    {
                        Level = f.HierarchyLevel,
                    },
                    Coordinates = new Coordinates
                    {
                        Latitude = f.CoordinatesLatitude,
                        Longitude = f.CoordinatesLongitude,
                    },
                }).ToListAsync();

                return folders;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during folders finding {exception}", ex);
                return null;
            }
        }

        public async Task<FolderResponseDTO?> GetFolder(int folderId)
        {
            try
            {
                var query = db.Folders
                    .Where(f => f.Id == folderId)
                    .AsQueryable();

                var folder = await query.Select(f => new FolderResponseDTO
                {
                    Id = f.Id,
                    OrganizationId = f.OrganizationId,
                    ParentsId = f.ParentId,
                    CameraCount = f.CameraCount,
                    Title = f.Title,
                    Hierarchy = new Hierarchy
                    {
                        Level = f.HierarchyLevel,
                    },
                    Coordinates = new Coordinates
                    {
                        Latitude = f.CoordinatesLatitude,
                        Longitude = f.CoordinatesLongitude,
                    },
                }).FirstOrDefaultAsync();

                return folder;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during folder finding {exception}", ex);
                return null;
            }
        }
    }
}
