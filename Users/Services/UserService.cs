using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Users.DTO.RequestDTO;
using BackEnd.Users.DTO.ResponseDTO;
using BackEnd.Utils.Roles;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BackEnd.Users.Services
{
    public class UserService(ILogger<UserService> logger, MyDbContext db)
    {
        public async Task<List<UserResponseDTO>?> GetUsers()
        {
            try
            {
                var users = await db.Users
                    .Include(u => u.M2mUsersOrganizations)
                    .Include(u => u.M2mUsersFolders)
                    .Select(u => new UserResponseDTO
                    {
                        Id = u.Id,
                        Login = u.Login,
                        Name = u.Name,
                        Email = u.Email,
                        Phone = u.Phone,
                        Note = u.Note,
                        MaxSessions = u.MaxSessions,
                        Disabled = u.Disabled,
                        AccessLevel = u.AccessLevel,
                        IsLoggedIn = u.IsLoggedIn,
                        Organizations = u.M2mUsersOrganizations != null ? u.M2mUsersOrganizations.Select(uo => new DTO.ResponseDTO.Organizations
                        {
                            Id = uo.OrganizationId,
                            // Title = db.Organizations.Find(uo.OrganizationId).Title,
                            IsMember = uo.IsMember,
                            IsAdmin = uo.IsAdmin,
                        }).ToList() : null,
                        Folders = u.M2mUsersFolders != null ? u.M2mUsersFolders.Select(uf => new DTO.ResponseDTO.Folders
                        {
                            Id = uf.FolderId,
                            CanView = uf.CanView,
                        }).ToList() : null,
                    }).ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<UserResponseDTO?> GetUser(int userId)
        {
            try
            {
                var user = await db.Users
                    .Include(u => u.M2mUsersOrganizations)
                    .Include(u => u.M2mUsersFolders)
                    .Where(u => u.Id == userId)
                    .Select(u => new UserResponseDTO
                    {
                        Id = u.Id,
                        Login = u.Login,
                        Name = u.Name,
                        Email = u.Email,
                        Phone = u.Phone,
                        Note = u.Note,
                        MaxSessions = u.MaxSessions,
                        Disabled = u.Disabled,
                        AccessLevel = u.AccessLevel,
                        IsLoggedIn = u.IsLoggedIn,
                        Organizations = u.M2mUsersOrganizations != null ? u.M2mUsersOrganizations.Select(uo => new DTO.ResponseDTO.Organizations
                        {
                            Id = uo.OrganizationId,
                            // Title = db.Organizations.Find(uo.OrganizationId).Title,
                            IsMember = uo.IsMember,
                            IsAdmin = uo.IsAdmin,
                        }).ToList() : null,
                        Folders = u.M2mUsersFolders != null ? u.M2mUsersFolders.Select(uf => new DTO.ResponseDTO.Folders
                        {
                            Id = uf.FolderId,
                            CanView = uf.CanView,
                        }).ToList() : null,
                    }).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<UserResponseDTO?> CreateUser(CreateUserDTO dto)
        {
            try
            {
                var passwordHash = CreateHashCode(dto.Password);

                if (!RoleType.AllRoles.Contains(dto.AccessLevel))
                {
                    throw new Exception("Bad Request");
                }

                var user = new UsersEntity
                {
                    Login = dto.Login,
                    Password = passwordHash,
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Note = dto.Note,
                    MaxSessions = dto.MaxSession,
                    Disabled = dto.Disabled,
                    AccessLevel = dto.AccessLevel,
                    IsLoggedIn = false,

                    Token = $"thisIsPersonalUserToken{dto.Login}",
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                if (dto.Organizations != null)
                    foreach (var org in dto.Organizations)
                    {
                        var userOrganization = new M2mUsersOrganizationsEntity
                        {
                            UserId = user.Id,
                            OrganizationId = org.Id,
                            IsMember = org.IsMember,
                            IsAdmin = org.IsAdmin,
                        };

                        db.M2mUsersOrganizations.Add(userOrganization);

                        var organization = await db.Organizations.FindAsync(org.Id);

                        if (organization != null) organization.UserCount++;
                    }

                await db.SaveChangesAsync();

                if (dto.Folders != null)
                    foreach (var fold in dto.Folders)
                    {
                        var userFolder = new M2mUsersFoldersEntity
                        {
                            UserId = user.Id,
                            FolderId = fold.Id,
                            CanView = fold.CanView,
                        };

                        db.M2mUsersFolders.Add(userFolder);
                    }

                await db.SaveChangesAsync();

                return await GetUser((int)user.Id);
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<UserResponseDTO?> ChangeUser(UserRequestDTO dto, int userId)
        {
            try
            {
                var user = await db.Users.FindAsync((uint)userId);

                if (user is null)
                {
                    return null;
                }

                if (dto.Login != null) user.Login = dto.Login;
                if (dto.Name != null) user.Name = dto.Name;
                if (dto.Email != null) user.Email = dto.Email;
                if (dto.Phone != null) user.Phone = dto.Phone;
                if (dto.Note != null) user.Note = dto.Note;
                if (dto.MaxSessions != null) user.MaxSessions = dto.MaxSessions.Value;
                if (dto.Disabled != null) user.Disabled = dto.Disabled.Value;
                if (dto.AccessLevel != null && RoleType.AllRoles.Contains(dto.AccessLevel)) user.AccessLevel = dto.AccessLevel;

                if (dto.Organizations != null)
                    foreach (var org in dto.Organizations)
                    {
                        var organizationUser = await db.M2mUsersOrganizations
                            .Include(uo => uo.Organization)
                            .FirstOrDefaultAsync(uo => uo.UserId == userId && uo.OrganizationId == org.Id);

                        if (organizationUser != null)
                        {
                            if (organizationUser.IsMember != org.IsMember)
                            {
                                if (org.IsMember)
                                {
                                    organizationUser.Organization.UserCount++;
                                }
                                else
                                {
                                    organizationUser.Organization.UserCount--;
                                }

                                organizationUser.IsMember = org.IsMember;
                            }

                            organizationUser.IsAdmin = org.IsAdmin;
                        }
                        else
                        {
                            var newOrganizationUser = new M2mUsersOrganizationsEntity
                            {
                                UserId = user.Id,
                                OrganizationId = org.Id,
                                IsMember = org.IsMember,
                                IsAdmin = org.IsAdmin,
                            };

                            db.M2mUsersOrganizations.Add(newOrganizationUser);

                            if (newOrganizationUser.IsMember)
                            {
                                var organization = await db.Organizations.FindAsync(org.Id);

                                if (organization != null)
                                    organization.UserCount++;
                            }
                        }
                    }

                await db.SaveChangesAsync();

                if (dto.Folders != null)
                    foreach (var fold in dto.Folders)
                    {
                        var folderUser = await db.M2mUsersFolders
                            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FolderId == fold.Id);

                        if (folderUser != null)
                        {
                            folderUser.CanView = fold.CanView;
                        }
                        else
                        {
                            var userFolder = new M2mUsersFoldersEntity
                            {
                                UserId = user.Id,
                                FolderId = fold.Id,
                                CanView = fold.CanView,
                            };

                            db.M2mUsersFolders.Add(userFolder);
                        }
                    }

                await db.SaveChangesAsync();

                return await GetUser(userId);
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteUser(int userId)
        {
            try
            {
                var user = await db.Users.FindAsync((uint)userId);

                if (user is null)
                {
                    return false;
                }

                var userOrganizations = await db.M2mUsersOrganizations
                    .Where(uo => uo.UserId == userId)
                    .ToListAsync();

                foreach (var us in userOrganizations)
                {
                    var organization = await db.Organizations.FindAsync(us.OrganizationId);
                    if (organization != null)
                    {
                        organization.UserCount--;
                        db.Organizations.Update(organization);
                    }
                }

                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }


        private string CreateHashCode(string input)
        {
            string hash = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                hash = builder.ToString();
            }
            return hash;
        }
    }
}
