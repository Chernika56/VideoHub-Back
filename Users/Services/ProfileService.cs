using BackEnd.Authorization.Services;
using BackEnd.DB.Context;
using BackEnd.Folders.DTO.RequestDTO;
using BackEnd.Users.DTO.RequestDTO;
using BackEnd.Users.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Users.Services
{
    public class ProfileService(ILogger<UserService> logger, MyDbContext db, AuthHelper authHelper)
    {
        public async Task<ProfileResponseDTO?> GetProfile()
        {
            try
            {
                var user = await db.Users
                    .Where(u => u.Login == authHelper.GetCurrentUserLogin())
                    .Select(u => new ProfileResponseDTO
                    {
                        Login = u.Login,
                        Name = u.Name,
                        Email = u.Email,
                        Phone = u.Phone,
                        Note = u.Note,
                        MaxSession = u.MaxSessions
                    }).FirstOrDefaultAsync();


                return user;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during user finding {exception}", ex);
                return null;
            }
        }

        public async Task<ProfileResponseDTO?> ChangeProfile(ProfileRequestDTO dto)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                user!.Name = dto.Name;
                user!.Email = dto.Email;
                user!.Phone = dto.Phone;
                user!.Note = dto.Note;

                await db.SaveChangesAsync();

                return new ProfileResponseDTO
                {
                    Login = user.Login,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Note = user.Note,
                    MaxSession = user.MaxSessions
                };
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during user finding {exception}", ex);
                return null;
            }
        }

        public async Task<List<MyOrganizationsResponseDTO>?> GetMyOrganizations()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var query = db.Organizations
                    .Include(o => o.M2mUsersOrganizations)
                    .AsQueryable();

                var allowedOrganizations = await db.M2mUsersOrganizations
                       .Where(uo => uo.UserId == user.Id && uo.IsMember)
                       .Select(uo => uo.OrganizationId)
                       .ToListAsync();

                query = query.Where(o => allowedOrganizations.Contains(o.Id));

                var organizations = await query
                    .Select(o => new MyOrganizationsResponseDTO
                    {
                        Id = o.Id,
                        Title = o.Title,
                        Stats = new Organizations.DTO.ResponseDTO.Stats
                        {
                            UserCount = o.UserCount,
                            CameraCount = o.CameraCount,
                            MosaicCount = o.MosaicCount
                        },
                        Limits = new Organizations.DTO.ResponseDTO.Limits
                        {
                            CameraLimit = o.CameraLimit,
                            UserLimit = o.UserLimit
                        },
                        IsDefault = o.IsDefault,
                        OwnerId = o.OwnerId,
                        UserPermissions = new DTO.ResponseDTO.Permissions
                        {
                            IsAdmin = o.M2mUsersOrganizations!.FirstOrDefault(uo => uo.UserId == user.Id)!.IsAdmin,
                            IsMember = o.M2mUsersOrganizations!.FirstOrDefault(uo => uo.UserId == user.Id)!.IsMember,
                        }
                    }).ToListAsync();

                return organizations;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organizations finding {exception}", ex);
                return null;
            }
        }

        public async Task<List<MyFoldersResponseDTO>?> GetMyFoldersInOrganization(uint organization_Id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var query = db.Folders
                    .Include(f => f.M2mUsersFolders)
                    .Where(f => f.OrganizationId == organization_Id)
                    .AsQueryable();

                var allowedFolders = await db.M2mUsersFolders
                       .Where(uf => uf.UserId == user.Id && uf.CanView)
                       .Select(uf => uf.FolderId)
                       .ToListAsync();

                query = query.Where(f => allowedFolders.Contains(f.Id));

                var folders = await query
                    .Select(f => new MyFoldersResponseDTO
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
                        CanView = f.M2mUsersFolders!.FirstOrDefault(uf => uf.UserId == user.Id)!.CanView,
                    }).ToListAsync();

                return folders;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during folders finding {exception}", ex);
                return null;
            }
        }


        public async Task<List<MyFoldersResponseDTO>?> GetMyFolders()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var query = db.Folders
                    .Include(f => f.M2mUsersFolders)
                    .AsQueryable();

                var allowedFolders = await db.M2mUsersFolders
                       .Where(uf => uf.UserId == user.Id && uf.CanView)
                       .Select(uf => uf.FolderId)
                       .ToListAsync();

                query = query.Where(f => allowedFolders.Contains(f.Id));

                var folders = await query
                    .Select(f => new MyFoldersResponseDTO
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
                        CanView = f.M2mUsersFolders!.FirstOrDefault(uf => uf.UserId == user.Id)!.CanView,
                    }).ToListAsync();

                return folders;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during folders finding {exception}", ex);
                return null;
            }
        }

        public async Task<List<MyMessagesResponseDTO>?> GetMyMessages()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var messages = await db.Messages
                    .Where(m => m.UserId == user.Id)
                    .Select(m => new MyMessagesResponseDTO
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Body = m.Body,
                        Type = m.Type,
                        IsPush = m.IsPush,
                        IsDashboard = m.IsDashboard,
                        IsDeleted = m.IsDeleted,
                        WasRead = m.WasRead,
                        SenderId = m.SenderId,
                        UserId = m.UserId
                    }).ToListAsync();

                return messages;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during messages finding {exception}", ex);
                return null;
            }
        }
    }
}
