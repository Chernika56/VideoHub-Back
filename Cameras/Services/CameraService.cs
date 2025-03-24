using BackEnd.Authorization.Services;
using BackEnd.Cameras.DTO.RequestDTO;
using BackEnd.Cameras.DTO.ResponseDTO;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Presets.DTO.ResponseDTO;
using BackEnd.ServerServices;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Cameras.Services
{
    public class CameraService(ILogger<CameraService> logger, MyDbContext db, AuthHelper authHelper, IStreamerService streamerService)
    {
        public async Task<List<CameraResponseDTO>?> GetCameras(string? folder_id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var query = db.Cameras
                    .Include(c => c.Folder)
                    .Include(c => c.Preset)
                    .Include(c => c.Streamer)
                    .Include(c => c.M2MUsersCameras)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(folder_id))
                {
                    var folderIds = folder_id.Split(',')
                        .Select(id => uint.TryParse(id, out var parsedId) ? parsedId : (uint?)null)
                        .Where(id => id.HasValue)
                        .Select(id => id!.Value)
                        .Distinct()
                        .ToList();

                    if (folderIds.Any())
                    {
                        var allowedFolders = await db.M2mUsersFolders
                            .Where(uf => uf.UserId == user.Id && uf.CanView && folderIds.Contains(uf.FolderId))
                            .Select(uf => uf.FolderId)
                            .ToListAsync();

                        query = query.Where(c => allowedFolders.Contains(c.FolderId));
                    }
                }
                else
                {
                    var allowedFolders = await db.M2mUsersFolders
                        .Where(uf => uf.UserId == user.Id && uf.CanView)
                        .Select(uf => uf.FolderId)
                        .ToListAsync();

                    query = query.Where(c => allowedFolders.Contains(c.FolderId));
                }

                var cameras = await MakeListDTO(query, user);

                return cameras;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during cameras finding {exception}", ex);
                return null;
            }
        }

        public async Task<CameraResponseDTO?> GetCamera(string cameraName)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var query = db.Cameras
                    .Include(c => c.Folder)
                    .Include(c => c.Preset)
                    .Include(c => c.Streamer)
                    .Include(c => c.M2MUsersCameras)
                    .Where(c => c.Name == cameraName)
                    .AsQueryable();

                var camera = await MakeDTO(query, user);

                return camera;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during camera finding {exception}", ex);
                return null;
            }
        }

        public async Task<CameraResponseDTO?> ChangeCamera(CameraRequestDTO dto, string cameraName)
        {
            try
            {
                var data = new
                {
                    name = cameraName,
                    comment = dto.Comment,
                    dvr = new
                    {
                        reference = dto.DVRPath,
                        expiration = dto.DVRDepth * 24 * 60 * 60,
                        storage_limit = dto.DVRSpace,
                    },
                    //MotionDetectorEnabled
                    meta = new
                    {
                        onvif_url = dto.OnvifURL,
                        onvif_profile = dto.OnvifProfile
                    },
                    inputs = new[]
                    {
                        new
                        {
                            url = dto.StreamUrl
                        }
                    },
                    template = "globals",
                    title = dto.Title,
                    on_play = new
                    {
                        url = "auth://vsaas"
                    },
                    prerush = 1,
                    disabled = false,
                };

                var response = await streamerService.UpdateStreamAsync(cameraName, data);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Bad response from server: {response.StatusCode}");
                }


                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == authHelper.GetCurrentUserLogin());

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var camera = await db.Cameras
                    .Include(c => c.M2MUsersCameras)
                    .FirstOrDefaultAsync(c => c.Name == cameraName);

                if (camera != null)
                {
                    if (dto.Comment != null) camera.Comment = dto.Comment;
                    if (dto.Coordinates != null) camera.Coordinates = dto.Coordinates;
                    if (dto.DVRDepth.HasValue) camera.DVRDepth = dto.DVRDepth.Value;
                    if (dto.DVRLockDays.HasValue) camera.DVRLockDays = dto.DVRLockDays.Value;
                    if (dto.DVRPath != null) camera.DVRPath = dto.DVRPath;
                    if (dto.DVRSpace.HasValue) camera.DVRSpace = dto.DVRSpace.Value;
                    if (dto.MotionDetectorEnabled.HasValue) camera.MotionDetectorEnabled = dto.MotionDetectorEnabled.Value;
                    if (dto.OnvifProfile != null) camera.OnvifProfile = dto.OnvifProfile;
                    if (dto.OnvifPTZ != null) camera.OnvifPTZ = dto.OnvifPTZ;
                    if (dto.OnvifURL != null) camera.OnvifURL = dto.OnvifURL;
                    if (dto.Permissions != null)
                    {
                        if (dto.Permissions.View != null) camera.View = dto.Permissions.View;
                        if (dto.Permissions.Edit != null) camera.Edit = dto.Permissions.Edit;
                        if (dto.Permissions.PTZ != null) camera.PTZ = dto.Permissions.PTZ;
                        if (dto.Permissions.DVR != null) camera.DVR = dto.Permissions.DVR;
                        if (dto.Permissions.DVRDepthLimit != null) camera.DVRDepthLimit = dto.Permissions.DVRDepthLimit.Value;
                        if (dto.Permissions.Actions != null) camera.Actions = dto.Permissions.Actions;
                    }
                    if (dto.PostalAddress != null) camera.PostalAddress = dto.PostalAddress;
                    if (dto.StreamUrl != null) camera.StreamUrl = dto.StreamUrl;
                    if (dto.SubStreamUrl != null) camera.SubStreamUrl = dto.SubStreamUrl;
                    if (dto.Title != null) camera.Title = dto.Title;

                    if ((dto.FolderId.HasValue) && (dto.FolderId != camera.FolderId))
                    {
                        var folder = await db.Folders
                            .Include(f => f.Organization)
                            .FirstOrDefaultAsync(f => f.Id == camera.FolderId);

                        if (folder != null)
                        {
                            folder.CameraCount -= 1;
                            if (folder.Organization != null)
                            {
                                folder.Organization.CameraCount -= 1;
                            }
                        }

                        camera.FolderId = dto.FolderId.Value;

                        var newFolder = await db.Folders
                            .Include(f => f.Organization)
                            .FirstOrDefaultAsync(f => f.Id == camera.FolderId);

                        if (newFolder != null)
                        {
                            newFolder.CameraCount += 1;
                            if (newFolder.Organization != null)
                            {
                                newFolder.Organization.CameraCount += 1;
                                camera.OrganizationId = newFolder.OrganizationId;
                            }
                        }
                    }

                    if (dto.PresetId.HasValue) camera.PresetId = dto.PresetId.Value;
                    if (dto.StreamerId.HasValue) camera.StreamerId = dto.StreamerId.Value;

                    db.Update(camera);
                }
                else
                {
                    camera = new CamerasEntity
                    {
                        Name = cameraName,
                        Comment = dto.Comment ?? "",
                        Coordinates = dto.Coordinates ?? "",
                        DVRDepth = dto.DVRDepth ?? 0,
                        DVRLockDays = dto.DVRLockDays ?? 0,
                        DVRPath = dto.DVRPath,
                        DVRSpace = dto.DVRSpace ?? 0,
                        MotionDetectorEnabled = dto.MotionDetectorEnabled ?? false,
                        OnvifProfile = dto.OnvifProfile ?? null,
                        OnvifURL = dto.OnvifURL ?? null,
                        OnvifPTZ = dto.OnvifPTZ ?? false,
                        View = dto.Permissions?.View ?? false,
                        Edit = dto.Permissions?.Edit ?? false,
                        PTZ = dto.Permissions?.PTZ ?? false,
                        DVR = dto.Permissions?.DVR ?? false,
                        DVRDepthLimit = dto.Permissions?.DVRDepthLimit ?? 0,
                        Actions = dto.Permissions?.Actions ?? false,
                        PostalAddress = dto.PostalAddress ?? null,
                        StreamUrl = dto.StreamUrl!,
                        SubStreamUrl = dto.SubStreamUrl ?? null,
                        Title = dto.Title!,

                        PresetId = dto.PresetId ?? 0,
                        StreamerId = dto.StreamerId ?? 0,
                        FolderId = dto.FolderId ?? 0,

                        Enabled = true,
                        LastEventTime = DateTimeOffset.UtcNow,
                    };

                    var folder = await db.Folders
                            .Include(f => f.Organization)
                            .FirstOrDefaultAsync(f => f.Id == camera.FolderId);

                    if (folder != null)
                    {
                        folder.CameraCount += 1;
                        if (folder.Organization != null)
                        {
                            folder.Organization.CameraCount += 1; // предусмотреть превышение лимита
                            camera.OrganizationId = folder!.OrganizationId; // при создании бд, нужно создать 0 папку и 0 организцию
                        }
                    }

                    db.Cameras.Add(camera);

                }

                await db.SaveChangesAsync();

                var query = db.Cameras
                    .Include(c => c.Folder)
                    .Include(c => c.Preset)
                    .Include(c => c.Streamer)
                    .Include(c => c.M2MUsersCameras)
                    .Where(c => c.Name == cameraName)
                    .AsQueryable();

                var result = await MakeDTO(query, user);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during camera finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeleteCamera(string cameraName)
        {
            try
            {
                var response = await streamerService.DeleteStreamAsync(cameraName);

                var camera = await db.Cameras
                    .Include(c => c.Folder)
                    .ThenInclude(f => f.Organization)
                    .FirstOrDefaultAsync(c => c.Name == cameraName);

                if (camera == null)
                {
                    return false;
                }

                if (camera.Folder != null)
                {
                    camera.Folder.CameraCount -= 1;

                    if (camera.Folder.Organization != null)
                    {
                        camera.Folder.Organization.CameraCount -= 1;
                    }
                }

                db.Cameras.Remove(camera);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during camera finding {exception}", ex);
                return null;
            }
        }

        private async Task<CameraResponseDTO?> MakeDTO(IQueryable<CamerasEntity> query, UsersEntity user)
        {
            var camera = await query
                    .Select(c => new CameraResponseDTO
                    {
                        Comment = c.Comment,
                        Coordinates = c.Coordinates ?? "",
                        DVRDepth = c.DVRDepth,
                        DVRLockDays = c.DVRLockDays,
                        DVRPath = c.DVRPath,
                        DVRSpace = c.DVRSpace,
                        Enabled = c.Enabled,
                        FolderCoordinates = c.Folder != null ? (c.Folder.CoordinatesLatitude.ToString() + " " + c.Folder.CoordinatesLongitude.ToString()) : null,
                        FolderId = c.FolderId,
                        LastEventTime = c.LastEventTime.ToUnixTimeSeconds(),
                        MotionDetectorEnabled = c.MotionDetectorEnabled,
                        Name = c.Name,
                        OnvifProfile = c.OnvifProfile,
                        OnvifRTZ = c.OnvifPTZ,
                        OnvifURL = c.OnvifURL,
                        OrganizationId = c.OrganizationId,
                        Permissions = new DTO.ResponseDTO.Permissions
                        {
                            View = c.View,
                            Edit = c.Edit,
                            PTZ = c.PTZ,
                            DVR = c.DVR,
                            DVRDepthLimit = c.DVRDepthLimit,
                            Actions = c.Actions
                        },
                        PlaybackConfig = new PlaybackConfig
                        {
                            Token = user!.Token
                        },
                        PostalAddress = c.PostalAddress,
                        Preset = new PresetResponseDTO
                        {
                            Id = c.Preset.Id,
                            Title = c.Preset.Title,
                            DVRDepth = c.Preset.DVRDepth,
                            DVRLockDays = c.Preset.DVRLockDays,
                            DVRSpace = c.Preset.DVRSpace,
                            IsAdjustable = c.Preset.IsAdjustable,
                            IsDefault = c.Preset.IsDefault,
                            PreciseTrumbnailsDays = c.Preset.PreciseTrumbnailsDays,
                            IsDeleted = c.Preset.IsDeleted
                        },
                        PresetId = c.PresetId,
                        StreamUrl = c.StreamUrl,
                        StreamerId = c.StreamerId,
                        SubStreamUrl = c.SubStreamUrl,
                        Title = c.Title,
                        UserAttributes = c.M2MUsersCameras
                            .Where(uc => uc.UserId == user!.Id)
                            .Select(uc => new DTO.ResponseDTO.UserAttributes
                            {
                                Favorite = uc.Favorite,
                                MotionAlarm = uc.MotionAlarm
                            })
                            .FirstOrDefault() ?? new DTO.ResponseDTO.UserAttributes { Favorite = false, MotionAlarm = false }
                    }).FirstOrDefaultAsync();

            return camera;
        }

        private async Task<List<CameraResponseDTO>?> MakeListDTO(IQueryable<CamerasEntity> query, UsersEntity user)
        {
            var cameras = await query
                    .Select(c => new CameraResponseDTO
                    {
                        Comment = c.Comment,
                        Coordinates = c.Coordinates ?? "",
                        DVRDepth = c.DVRDepth,
                        DVRLockDays = c.DVRLockDays,
                        DVRPath = c.DVRPath,
                        DVRSpace = c.DVRSpace,
                        Enabled = c.Enabled,
                        FolderCoordinates = c.Folder != null ? (c.Folder.CoordinatesLatitude.ToString() + " " + c.Folder.CoordinatesLongitude.ToString()) : null,
                        FolderId = c.FolderId,
                        LastEventTime = c.LastEventTime.ToUnixTimeSeconds(),
                        MotionDetectorEnabled = c.MotionDetectorEnabled,
                        Name = c.Name,
                        OnvifProfile = c.OnvifProfile,
                        OnvifRTZ = c.OnvifPTZ,
                        OnvifURL = c.OnvifURL,
                        OrganizationId = c.OrganizationId,
                        Permissions = new DTO.ResponseDTO.Permissions
                        {
                            View = c.View,
                            Edit = c.Edit,
                            PTZ = c.PTZ,
                            DVR = c.DVR,
                            DVRDepthLimit = c.DVRDepthLimit,
                            Actions = c.Actions
                        },
                        PlaybackConfig = new PlaybackConfig
                        {
                            Token = user!.Token
                        },
                        PostalAddress = c.PostalAddress,
                        Preset = new PresetResponseDTO
                        {
                            Id = c.Preset.Id,
                            Title = c.Preset.Title,
                            DVRDepth = c.Preset.DVRDepth,
                            DVRLockDays = c.Preset.DVRLockDays,
                            DVRSpace = c.Preset.DVRSpace,
                            IsAdjustable = c.Preset.IsAdjustable,
                            IsDefault = c.Preset.IsDefault,
                            PreciseTrumbnailsDays = c.Preset.PreciseTrumbnailsDays,
                            IsDeleted = c.Preset.IsDeleted
                        },
                        PresetId = c.PresetId,
                        StreamUrl = c.StreamUrl,
                        StreamerId = c.StreamerId,
                        SubStreamUrl = c.SubStreamUrl,
                        Title = c.Title,
                        UserAttributes = c.M2MUsersCameras
                            .Where(uc => uc.UserId == user!.Id)
                            .Select(uc => new DTO.ResponseDTO.UserAttributes
                            {
                                Favorite = uc.Favorite,
                                MotionAlarm = uc.MotionAlarm
                            })
                            .FirstOrDefault() ?? new DTO.ResponseDTO.UserAttributes { Favorite = false, MotionAlarm = false }
                    }).ToListAsync();

            return cameras;
        }
    }
}
