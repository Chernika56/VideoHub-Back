using BackEnd.Authorization.Services;
using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Organizations.Services
{
    public class OrganizationUsersService(ILogger<CameraService> logger, MyDbContext db)
    {
        public async Task<List<OrganizationUserResponseDTO>?> GetOrganizationUsers(uint organizationId)
        {
            try
            {
                var query = db.M2mUsersOrganizations
                    .Include(uo => uo.User)
                        .ThenInclude(u => u.M2mUsersFolders)
                    .Where(uo => uo.OrganizationId == organizationId)
                    .AsQueryable();

                var users = await query.Select(uo => new OrganizationUserResponseDTO
                {
                    Id = uo.UserId,
                    Name = uo.User.Name,
                    Email = uo.User.Email,
                    Permissions = new Permissions
                    {
                        Folders = uo.User.M2mUsersFolders
                            .Select(uf => new FolderPermissions
                            {
                                Id = uf.FolderId,
                                CanView = uf.CanView,
                            }).ToList(),
                        Organization = new OrganizationPermissions
                        {
                            IsAdmin = uo.IsAdmin,
                            IsMember = uo.IsMember,
                        },
                    },
                }).ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationUserResponseDTO?> GetOrganizationUser(uint organizationId, uint userId)
        {
            try
            {
                var query = db.M2mUsersOrganizations
                    .Include(uo => uo.User)
                        .ThenInclude(u => u.M2mUsersFolders)
                    .Where(uo => uo.OrganizationId == organizationId && uo.UserId == userId)
                    .AsQueryable();

                var user = await query.Select(uo => new OrganizationUserResponseDTO
                {
                    Id = uo.UserId,
                    Name = uo.User.Name,
                    Email = uo.User.Email,
                    Permissions = new Permissions
                    {
                        Folders = uo.User.M2mUsersFolders
                            .Select(uf => new FolderPermissions
                            {
                                Id = uf.FolderId,
                                CanView = uf.CanView,
                            }).ToList(),
                        Organization = new OrganizationPermissions
                        {
                            IsAdmin = uo.IsAdmin,
                            IsMember = uo.IsMember,
                        },
                    },
                }).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationUserResponseDTO?> ChangeOrganizationUser(OrganizationUserRequestDTO dto, uint organizationId, uint userId)
        {
            try
            {
                var user = await db.M2mUsersOrganizations.FirstOrDefaultAsync(uo => uo.OrganizationId == organizationId && uo.UserId == userId);

                if (user != null)
                {
                    if ((dto.IsMember != null) && (dto.IsMember.Value != user.IsMember))
                    {
                        var organization = await db.Organizations.FindAsync(organizationId);

                        if (organization != null)
                            if (dto.IsMember.Value)
                            {
                                organization.UserCount += 1;
                            }
                            else
                            {
                                organization.UserCount -= 1;
                            }

                        user.IsMember = dto.IsMember.Value;
                    }

                    if (dto.IsAdmin != null) user.IsAdmin = dto.IsAdmin.Value;
                }
                else
                {
                    user = new M2mUsersOrganizationsEntity
                    {
                        UserId = userId,
                        OrganizationId = organizationId,
                        IsAdmin = dto.IsAdmin ?? false,
                        IsMember = dto.IsMember ?? false,
                    };

                    if (user.IsMember)
                    {
                        var organization = await db.Organizations.FindAsync(organizationId);

                        if (organization != null)
                            organization.UserCount += 1;
                    }

                    db.M2mUsersOrganizations.Add(user);
                }

                await db.SaveChangesAsync();

                return await GetOrganizationUser(organizationId, userId);
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteUserFromOrganization(uint organizationId, uint userId)
        {
            try
            {
                await db.M2mUsersOrganizations
                    .Where(uo => uo.OrganizationId == organizationId && uo.UserId == userId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(uo => uo.IsMember, false)
                        .SetProperty(uo => uo.IsAdmin, false)
                    );

                await db.M2mUsersFolders
                    .Where(uf => uf.UserId == userId && uf.Folder.OrganizationId == organizationId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(uf => uf.CanView, false));

                var organization = await db.Organizations.FindAsync(organizationId);

                if (organization != null)
                    organization.UserCount -= 1;

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
