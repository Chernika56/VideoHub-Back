using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Organizations.Services
{
    public class OrganizationFoldersService(ILogger<CameraService> logger, MyDbContext db)
    {
        public async Task<List<OrganizationFolderResponseDTO>?> GetOrganizationFolders(uint organizationId)
        {
            try
            {
                var query = db.Folders
                    .Where(f => f.OrganizationId == organizationId)
                    .AsQueryable();

                var folders = await query.Select(f => new OrganizationFolderResponseDTO
                {
                    Id = f.Id,
                    OrganizationId = f.OrganizationId,
                    ParentsId = f.ParentId,
                    CameraCount = f.CameraCount,
                    Title = f.Title,
                    Hierarchy = new Folders.DTO.ResponseDTO.Hierarchy
                    {
                        Level = f.HierarchyLevel,
                    },
                    Coordinates = new Folders.DTO.ResponseDTO.Coordinates
                    {
                        Latitude = f.CoordinatesLatitude,
                        Longitude = f.CoordinatesLongitude,
                    },
                }).ToListAsync();

                return folders;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationFolderResponseDTO?> CreateOrganizationFolder(OrganizationFolderRequestDTO dto, uint organizationId)
        {
            try
            {
                var folder = new FoldersEntity
                {
                    OrganizationId = dto.OrganizationId ?? 0,
                    ParentId = dto.ParentId ?? null,
                    Title = dto.Title ?? "",
                    HierarchyLevel = dto.Hierarchy?.Level ?? 0,
                    CoordinatesLatitude = dto.Coordinates?.Latitude ?? null,
                    CoordinatesLongitude = dto.Coordinates?.Longitude ?? null,

                    CameraCount = 0,
                };

                db.Folders.Add(folder);

                await db.SaveChangesAsync();

                var query = db.Folders
                    .Where(f => f.Id == folder.Id)
                    .AsQueryable();

                var result = await query.Select(f => new OrganizationFolderResponseDTO
                {
                    Id = f.Id,
                    OrganizationId = f.OrganizationId,
                    ParentsId = f.ParentId,
                    CameraCount = f.CameraCount,
                    Title = f.Title,
                    Hierarchy = new Folders.DTO.ResponseDTO.Hierarchy
                    {
                        Level = f.HierarchyLevel,
                    },
                    Coordinates = new Folders.DTO.ResponseDTO.Coordinates
                    {
                        Latitude = f.CoordinatesLatitude,
                        Longitude = f.CoordinatesLongitude,
                    },
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationFolderResponseDTO?> GetOrganizationFolder(uint organizationId, uint folderId)
        {
            try
            {
                var query = db.Folders
                    .Where(f => f.Id == folderId && f.OrganizationId == organizationId)
                    .AsQueryable();

                var folder = await query.Select(f => new OrganizationFolderResponseDTO
                {
                    Id = f.Id,
                    OrganizationId = f.OrganizationId,
                    ParentsId = f.ParentId,
                    CameraCount = f.CameraCount,
                    Title = f.Title,
                    Hierarchy = new Folders.DTO.ResponseDTO.Hierarchy
                    {
                        Level = f.HierarchyLevel,
                    },
                    Coordinates = new Folders.DTO.ResponseDTO.Coordinates
                    {
                        Latitude = f.CoordinatesLatitude,
                        Longitude = f.CoordinatesLongitude,
                    },
                }).FirstOrDefaultAsync();

                return folder;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationFolderResponseDTO?> ChangeOrganizationFolder(OrganizationFolderRequestDTO dto, uint organizationId, uint folderId)
        {
            try
            {
                var folder = await db.Folders
                    .Include(f => f.Organization)
                    .FirstOrDefaultAsync(f => f.Id == folderId && f.OrganizationId == organizationId);

                if (folder != null)
                {
                    if ((dto.OrganizationId != null) && (dto.OrganizationId != folder.OrganizationId))
                    {
                        if (folder.Organization != null)
        {
                            folder.Organization.CameraCount -= folder.CameraCount;
                        }

                        folder.OrganizationId = dto.OrganizationId.Value;

                        var newOrganization = await db.Organizations.FindAsync(dto.OrganizationId.Value);
                        if (newOrganization != null)
                        {
                            newOrganization.CameraCount += folder.CameraCount;
                        }
                    }

                    if ((dto.ParentId != null) && (dto.ParentId != folder.ParentId)) folder.ParentId = dto.ParentId.Value;
                    if (dto.Title != null) folder.Title = dto.Title;
                    if (dto.Hierarchy != null)
                    {
                        if (dto.Hierarchy.Level != null) folder.HierarchyLevel = dto.Hierarchy.Level.Value;
                    }
                    if (dto.Coordinates != null)
                    {
                        if (dto.Coordinates.Latitude != null) folder.CoordinatesLatitude = dto.Coordinates.Latitude.Value;
                        if (dto.Coordinates.Longitude != null) folder.CoordinatesLongitude = dto.Coordinates.Longitude.Value;
                    }

                    db.Update(folder);
                }
                else
                {
                    folder = new FoldersEntity
                    {
                        OrganizationId = dto.OrganizationId ?? 0,
                        ParentId = dto.ParentId ?? null,
                        Title = dto.Title ?? "",
                        HierarchyLevel = dto.Hierarchy?.Level ?? 0,
                        CoordinatesLatitude = dto.Coordinates?.Latitude ?? null,
                        CoordinatesLongitude = dto.Coordinates?.Longitude ?? null,

                        CameraCount = 0,
                    };

                    db.Folders.Add(folder);
                }

                await db.SaveChangesAsync();

                var query = db.Folders
                    .Where(f => f.Id == folderId)
                    .AsQueryable();

                var result = await query.Select(f => new OrganizationFolderResponseDTO
                {
                    Id = f.Id,
                    OrganizationId = f.OrganizationId,
                    ParentsId = f.ParentId,
                    CameraCount = f.CameraCount,
                    Title = f.Title,
                    Hierarchy = new Folders.DTO.ResponseDTO.Hierarchy
                    {
                        Level = f.HierarchyLevel,
                    },
                    Coordinates = new Folders.DTO.ResponseDTO.Coordinates
                    {
                        Latitude = f.CoordinatesLatitude,
                        Longitude = f.CoordinatesLongitude,
                    },
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteFolder(uint organizationId, uint folderId)
        {
            try
            {
                var folder = await db.Folders.FirstOrDefaultAsync(f => f.Id == folderId && f.OrganizationId == organizationId);

                if (folder == null)
                {
                    return false;
                }

                db.Folders.Remove(folder);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }
    }
}
