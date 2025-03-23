using BackEnd.Authorization.Services;
using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace BackEnd.Organizations.Services
{
    public class OrganizationFolderUserService(ILogger<CameraService> logger, MyDbContext db, AuthHelper authHelper)
    {
        public async Task<List<FolderUserResponseDTO>?> GetFolderUsers(uint organizationId, uint folderId)
        {
            try
            {
                var query = db.M2mUsersFolders
                    .Where(uf => uf.FolderId == folderId)
                    .AsQueryable();

                var users = await query.Select(uf => new FolderUserResponseDTO
                {
                    Id = uf.Id,
                    FolderId = uf.FolderId,
                    UserId = uf.UserId,
                    CanView = uf.CanView,
                }).ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during users finding {exception}", ex);
                return null;
            }
        }

        public async Task<FolderUserResponseDTO?> GetFolderUser(uint organizationId, uint folderId, uint userId)
        {
            try
            {
                var query = db.M2mUsersFolders
                    .Where(uf => uf.FolderId == folderId && uf.UserId == userId)
                    .AsQueryable();

                var user = await query.Select(uf => new FolderUserResponseDTO
                {
                    Id = uf.Id,
                    FolderId = uf.FolderId,
                    UserId = uf.UserId,
                    CanView = uf.CanView,
                }).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during users finding {exception}", ex);
                return null;
            }
        }

        public async Task<FolderUserResponseDTO?> ChangeFolderUser(FolderUserRequestDTO dto, uint organizationId, uint folderId, uint userId)
        {
            try
            {
                var user = await db.M2mUsersFolders.FirstOrDefaultAsync(uf => uf.FolderId == folderId && uf.UserId == userId);

                if (user != null)
                {
                    user.CanView = dto.CanView;
                }
                else
                {
                    user = new DB.Entities.M2mUsersFoldersEntity
                    {
                        FolderId = folderId,
                        UserId = userId,
                        CanView = dto.CanView,
                    };

                    db.M2mUsersFolders.Add(user);
                }

                await db.SaveChangesAsync();

                var query = db.M2mUsersFolders
                    .Where(uf => uf.FolderId == folderId && uf.UserId == userId)
                    .AsQueryable();

                var res = await query.Select(uf => new FolderUserResponseDTO
                {
                    Id = uf.Id,
                    FolderId = uf.FolderId,
                    UserId = uf.UserId,
                    CanView = uf.CanView,
                }).FirstOrDefaultAsync();

                return res;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during users finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteFolderUser(uint organizationId, uint folderId, uint userId)
        {
            try
            {
                var user = await db.M2mUsersFolders.FirstOrDefaultAsync(uf => uf.FolderId == folderId && uf.UserId == userId);

                if (user == null)
                {
                    return false;
                }

                db.M2mUsersFolders.Remove(user);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during users finding {exception}", ex);
                return null;
            }
        }
    }
}
