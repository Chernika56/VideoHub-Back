using BackEnd.Authorization.Services;
using BackEnd.Cameras.DTO.ResponseDTO;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Mosaics.DTO;
using BackEnd.Presets.DTO.ResponseDTO;
using BackEnd.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Mosaics.Services
{
    public class MosaicsService(ILogger<UserService> logger, MyDbContext db, AuthHelper authHelper)
    {
        public async Task<List<MosaicsResponseDTO>?> GetMosaics(string? organization_id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var query = db.Mosaics
                    .AsQueryable();

                if (!string.IsNullOrEmpty(organization_id))
                {
                    var organizationIds = organization_id.Split(',')
                        .Select(id => uint.TryParse(id, out var parsedId) ? parsedId : (uint?)null)
                        .Where(id => id.HasValue)
                        .Select(id => id!.Value)
                        .Distinct()
                        .ToList();

                    if (organizationIds.Any())
                    {
                        var allowedOrganizations = await db.M2mUsersOrganizations
                            .Where(uo => uo.UserId == user.Id && uo.IsMember && organizationIds.Contains(uo.OrganizationId))
                            .Select(uf => uf.OrganizationId)
                            .ToListAsync();

                        query = query.Where(m => allowedOrganizations.Contains(m.OrganizationId));
                    }
                }
                else
                {
                    var allowedOrganizations = await db.M2mUsersOrganizations
                        .Where(uo => uo.UserId == user.Id && uo.IsMember)
                        .Select(uf => uf.OrganizationId)
                        .ToListAsync();

                    query = query.Where(m => allowedOrganizations.Contains(m.OrganizationId));
                }

                var mosaics = await query
                    .Select(m => new MosaicsResponseDTO
                    {
                        Id = m.Id,
                        OrganizationId = m.OrganizationId,
                        Title = m.Title,
                        Type = m.Type,
                        Visible = m.Visible,
                    }).ToListAsync();

                return mosaics;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during mosaics finding {exception}", ex);
                return null;
            }
        }

        public async Task<MosaicResponseDTO?> GetMosaic(uint mosaic_id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                var mosaic = await db.Mosaics
                    .Include(m => m.M2mMosaicsCameras)
                        .ThenInclude(mc => mc.Camera)
                            .ThenInclude(c => c.Preset)
                    .Include(m => m.M2mMosaicsCameras)
                        .ThenInclude(mc => mc.Camera)
                            .ThenInclude(c => c.Folder)
                    .Include(m => m.M2mMosaicsCameras)
                        .ThenInclude(mc => mc.Camera)
                            .ThenInclude(c => c.Streamer)
                    .Include(m => m.M2mMosaicsCameras)
                        .ThenInclude(mc => mc.Camera)
                            .ThenInclude(c => c.M2MUsersCameras)
                    .Where(m => m.Id == mosaic_id)
                    .Select(m => new MosaicResponseDTO
                    {
                        Id = m.Id,
                        OrganizationId = m.OrganizationId,
                        Title = m.Title,
                        Type = m.Type,
                        Visible = m.Visible,
                        Cameras = m.M2mMosaicsCameras.Select(mc => new CameraResponseDTO
                        {
                            Id = mc.Camera.Id,
                            Comment = mc.Camera.Comment,
                            Coordinates = mc.Camera.Coordinates ?? "",
                            DVRDepth = mc.Camera.DVRDepth,
                            DVRLockDays = mc.Camera.DVRLockDays,
                            DVRPath = mc.Camera.Streamer != null ? mc.Camera.Streamer.DVRPath : null,
                            DVRSpace = mc.Camera.DVRSpace,
                            Enabled = mc.Camera.Enabled,
                            FolderCoordinates = mc.Camera.Folder != null ? (mc.Camera.Folder.CoordinatesLatitude.ToString() + " " + mc.Camera.Folder.CoordinatesLongitude.ToString()) : null,
                            FolderId = mc.Camera.FolderId,
                            LastEventTime = mc.Camera.LastEventTime.ToUnixTimeSeconds(),
                            MotionDetectorEnabled = mc.Camera.MotionDetectorEnabled,
                            Name = mc.Camera.Name,
                            OnvifProfile = mc.Camera.OnvifProfile,
                            OnvifRTZ = mc.Camera.OnvifPTZ,
                            OnvifURL = mc.Camera.OnvifURL,
                            OrganizationId = mc.Camera.OrganizationId,
                            Permissions = new Permissions
                            {
                                View = mc.Camera.View,
                                Edit = mc.Camera.Edit,
                                PTZ = mc.Camera.PTZ,
                                DVR = mc.Camera.DVR,
                                DVRDepthLimit = mc.Camera.DVRDepthLimit,
                                Actions = mc.Camera.Actions
                            },
                            PlaybackConfig = new PlaybackConfig
                            {
                                Token = user!.Token
                            },
                            PostalAddress = mc.Camera.PostalAddress,
                            Preset = new PresetResponseDTO
                            {
                                Id = mc.Camera.Preset.Id,
                                Title = mc.Camera.Preset.Title,
                                DVRDepth = mc.Camera.Preset.DVRDepth,
                                DVRLockDays = mc.Camera.Preset.DVRLockDays,
                                DVRSpace = mc.Camera.Preset.DVRSpace,
                                IsAdjustable = mc.Camera.Preset.IsAdjustable,
                                IsDefault = mc.Camera.Preset.IsDefault,
                                PreciseTrumbnailsDays = mc.Camera.Preset.PreciseTrumbnailsDays,
                                IsDeleted = mc.Camera.Preset.IsDeleted
                            },
                            PresetId = mc.Camera.PresetId,
                            StreamUrl = mc.Camera.StreamUrl,
                            StreamerId = mc.Camera.StreamerId,
                            SubStreamUrl = mc.Camera.SubStreamUrl,
                            Title = mc.Camera.Title,
                            UserAttributes = mc.Camera.M2MUsersCameras
                                .Where(uc => uc.UserId == user!.Id)
                                .Select(uc => new UserAttributes
                                {
                                    Favorite = uc.Favorite,
                                    MotionAlarm = uc.MotionAlarm
                                })
                            .FirstOrDefault() ?? new UserAttributes { Favorite = false, MotionAlarm = false }
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return mosaic;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during mosaics finding {exception}", ex);
                return null;
            }
        }

        public async Task<MosaicResponseDTO?> CreateMosaic(MosaicCreateDTO dto)
        {
            try
            {
                var mosaic = new MosaicsEntity
                {
                    OrganizationId = dto.OrganizationId,
                    Title = dto.Title,
                    Type = dto.Type,
                    Visible = dto.Visible,
                };

                db.Mosaics.Add(mosaic);
                await db.SaveChangesAsync();

                var organization = await db.Organizations.FindAsync(dto.OrganizationId);

                if (organization != null)
                {
                    organization.MosaicCount++;
                }

                foreach (var cam in dto.Cameras)
                {
                    var mosaicsCameras = new M2mMosaicsCamerasEntity
                    {
                        MosaicId = mosaic.Id,
                        CameraId = cam,
                    };

                    db.M2mMosaicsCameras.Add(mosaicsCameras);
                }

                await db.SaveChangesAsync();

                return await GetMosaic(mosaic.Id);
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during mosaics finding {exception}", ex);
                return null;
            }
        }

        public async Task<MosaicResponseDTO?> ChangeMosaic(MosaicRequestDTO dto, uint mosaicId)
        {
            try
            {
                var mosaic = await db.Mosaics
                    .Include(m => m.M2mMosaicsCameras)
                    .Include(m => m.Organization)
                    .FirstOrDefaultAsync(m => m.Id == mosaicId);

                if (mosaic != null)
                {
                    if (dto.OrganizationId != null && dto.OrganizationId != mosaic.OrganizationId)
                    {
                        mosaic.Organization.MosaicCount--;

                        mosaic.OrganizationId = dto.OrganizationId.Value;

                        var newOrganization = await db.Organizations.FindAsync(mosaic.OrganizationId);

                        if (newOrganization != null)
                        {
                            newOrganization.MosaicCount++;
                        }
                    }

                    if (dto.Title != null) mosaic.Title = dto.Title;
                    if (dto.Type != null) mosaic.Type = dto.Type;
                    if (dto.Visible != null) mosaic.Visible = dto.Visible.Value;

                    if (dto.Cameras != null)
                    {
                        var existingMosaicsCameras = await db.M2mMosaicsCameras
                            .Where(mc => mc.MosaicId == mosaicId)
                            .ToListAsync();

                        var existingCameras = existingMosaicsCameras.Select(mc => mc.CameraId).ToHashSet();
                        var newCameras = dto.Cameras?.ToHashSet() ?? new HashSet<uint>();

                        var camerasToRemove = existingMosaicsCameras.Where(mc => !newCameras.Contains(mc.CameraId)).ToList();

                        var camerasToAdd = newCameras.Except(existingCameras)
                            .Select(cameraId => new M2mMosaicsCamerasEntity
                            {
                                MosaicId = mosaicId,
                                CameraId = cameraId
                            })
                            .ToList();

                        db.M2mMosaicsCameras.RemoveRange(camerasToRemove);
                        db.M2mMosaicsCameras.AddRange(camerasToAdd);
                    }
                }
                else
                {
                    throw new Exception("Mosaic not found");
                }

                await db.SaveChangesAsync();

                return await GetMosaic(mosaicId);
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during mosaics finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteMosaic(uint mosaicId)
        {
            try
            {
                var mosaic = await db.Mosaics
                    .Include(m => m.Organization)
                    .FirstAsync(m => m.Id == mosaicId);

                if (mosaic is null)
                {
                    return false;
                }

                mosaic.Organization.MosaicCount--;

                db.Mosaics.Remove(mosaic);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during mosaics finding {exception}", ex);
                return null;
            }
        }
    }
}
