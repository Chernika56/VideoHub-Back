using BackEnd.Authorization.Services;
using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Organizations.DTO.RequestDTO;
using BackEnd.Organizations.DTO.ResponseDTO;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Organizations.Services
{
    public class OrganizationService(ILogger<CameraService> logger, MyDbContext db, AuthHelper authHelper)
    {
        public async Task<List<OrganizationResponseDTO>?> GetOrganizations()
        {
            try
            {
                var query = db.Organizations.AsQueryable();

                var organizations = await query.Select(o => new OrganizationResponseDTO
                {
                    Id = o.Id,
                    Title = o.Title,
                    Stats = new DTO.ResponseDTO.Stats
                    {
                        UserCount = o.UserCount,
                        CameraCount = o.CameraCount,
                        MosaicCount = o.MosaicCount,
                    },
                    Limits = new DTO.ResponseDTO.Limits
                    {
                        UserLimit = o.UserLimit,
                        CameraLimit = o.CameraLimit,
                    },
                    IsDefault = o.IsDefault,
                    OwnerId = o.OwnerId,
                }).ToListAsync();

                return organizations;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationResponseDTO?> CreateOrganization(OrganizationRequestDTO dto)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var organization = new OrganizationsEntity
                {
                    Title = dto.Title ?? "",
                    CameraLimit = dto.Limits?.CameraLimit ?? 0,
                    UserLimit = dto.Limits?.UserLimit ?? 0,
                    IsDefault = dto.IsDefault ?? false,
                    OwnerId = dto.OwnerId ?? user.Id,

                    UserCount = 0,
                    CameraCount = 0,
                    MosaicCount = 0,
                };

                db.Organizations.Add(organization);

                await db.SaveChangesAsync();

                var query = db.Organizations
                    .Where(o => o.Id == organization.Id)
                    .AsQueryable();

                var result = await query.Select(o => new OrganizationResponseDTO
                {
                    Id = o.Id,
                    Title = o.Title,
                    Stats = new DTO.ResponseDTO.Stats
                    {
                        UserCount = o.UserCount,
                        CameraCount = o.CameraCount,
                        MosaicCount = o.MosaicCount,
                    },
                    Limits = new DTO.ResponseDTO.Limits
                    {
                        UserLimit = o.UserLimit,
                        CameraLimit = o.CameraLimit,
                    },
                    IsDefault = o.IsDefault,
                    OwnerId = o.OwnerId,
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationResponseDTO?> GetOrganization(int organizationId)
        {
            try
            {
                var query = db.Organizations
                    .Where(o => o.Id == organizationId)
                    .AsQueryable();

                var organization = await query.Select(o => new OrganizationResponseDTO
                {
                    Id = o.Id,
                    Title = o.Title,
                    Stats = new DTO.ResponseDTO.Stats
                    {
                        UserCount = o.UserCount,
                        CameraCount = o.CameraCount,
                        MosaicCount = o.MosaicCount,
                    },
                    Limits = new DTO.ResponseDTO.Limits
                    {
                        UserLimit = o.UserLimit,
                        CameraLimit = o.CameraLimit,
                    },
                    IsDefault = o.IsDefault,
                    OwnerId = o.OwnerId,
                }).FirstOrDefaultAsync();

                return organization;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<OrganizationResponseDTO?> ChangeOrganization(OrganizationRequestDTO dto, int organizationId)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var organization = await db.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);

                if (organization != null)
                {
                    if (dto.Title != null) organization.Title = dto.Title;
                    if (dto.Limits != null)
                    {
                        if (dto.Limits.CameraLimit != null) organization.CameraLimit = dto.Limits.CameraLimit.Value;
                        if (dto.Limits.UserLimit != null) organization.UserLimit = dto.Limits.UserLimit.Value;
                    }
                    if (dto.IsDefault != null) organization.IsDefault = dto.IsDefault.Value;
                    if (dto.OwnerId != null) organization.OwnerId = dto.OwnerId.Value;

                    db.Update(organization);
                }
                else
                {
                    organization = new OrganizationsEntity
                    {
                        Title = dto.Title ?? "",
                        CameraLimit = dto.Limits?.CameraLimit ?? 0,
                        UserLimit = dto.Limits?.UserLimit ?? 0,
                        IsDefault = dto.IsDefault ?? false,
                        OwnerId = dto.OwnerId ?? user.Id,

                        UserCount = 0,
                        CameraCount = 0,
                        MosaicCount = 0,
                    };

                    db.Organizations.Add(organization);
                }

                await db.SaveChangesAsync();

                var query = db.Organizations
                    .Where(o => o.Id == organizationId)
                    .AsQueryable();

                var result = await query.Select(o => new OrganizationResponseDTO
                {
                    Id = o.Id,
                    Title = o.Title,
                    Stats = new DTO.ResponseDTO.Stats
                    {
                        UserCount = o.UserCount,
                        CameraCount = o.CameraCount,
                        MosaicCount = o.MosaicCount,
                    },
                    Limits = new DTO.ResponseDTO.Limits
                    {
                        UserLimit = o.UserLimit,
                        CameraLimit = o.CameraLimit,
                    },
                    IsDefault = o.IsDefault,
                    OwnerId = o.OwnerId,
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteOrganization(int organizationId)
        {
            try
            {
                var organization = await db.Organizations.FindAsync((uint)organizationId);
                if (organization == null)
                {
                    return false;
                }

                db.Organizations.Remove(organization);
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
