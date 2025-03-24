using BackEnd.Cameras.Services;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Presets.DTO.RequestDTO;
using BackEnd.Presets.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Presets.Services
{
    public class PresetService(ILogger<CameraService> logger, MyDbContext db)
    {
        public async Task<List<PresetResponseDTO>?> GetPresets()
        {
            try
            {
                var query = db.Presets.AsQueryable();

                var presets = await query.Select(p => new PresetResponseDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    DVRDepth = p.DVRDepth,
                    DVRLockDays = p.DVRLockDays,
                    DVRSpace = p.DVRSpace,
                    IsAdjustable = p.IsAdjustable,
                    IsDefault = p.IsDefault,
                    PreciseTrumbnailsDays = p.PreciseTrumbnailsDays,
                    IsDeleted = p.IsDeleted,
                }).ToListAsync();

                return presets;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during presets finding {exception}", ex);
                return null;
            }
        }

        public async Task<PresetResponseDTO?> GetPreset(int presetId)
        {
            try
            {
                var query = db.Presets
                    .Where(p => p.Id == presetId)
                    .AsQueryable();

                var preset = await query.Select(p => new PresetResponseDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    DVRDepth = p.DVRDepth,
                    DVRLockDays = p.DVRLockDays,
                    DVRSpace = p.DVRSpace,
                    IsAdjustable = p.IsAdjustable,
                    IsDefault = p.IsDefault,
                    PreciseTrumbnailsDays = p.PreciseTrumbnailsDays,
                    IsDeleted = p.IsDeleted,
                }).FirstOrDefaultAsync();

                return preset;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during presets finding {exception}", ex);
                return null;
            }
        }

        public async Task<PresetResponseDTO?> CreatePreset(PresetRequestDTO dto)
        {
            try
            {
                var preset = new PresetsEntity
                {
                    Title = dto.Title ?? "",
                    DVRDepth = dto.DVRDepth ?? 0,
                    DVRLockDays = dto.DVRLockDays ?? 0,
                    DVRSpace = dto.DVRSpace ?? 0,
                    IsAdjustable = dto.IsAdjustable ?? true,
                    IsDefault = dto.IsDefault ?? false,
                    PreciseTrumbnailsDays = dto.PreciseTrumbnailsDays ?? 0,
                    IsDeleted = false,
                };

                db.Presets.Add(preset);

                await db.SaveChangesAsync();

                var query = db.Presets
                    .Where(p => p.Id == preset.Id)
                    .AsQueryable();

                var result = await query.Select(p => new PresetResponseDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    DVRDepth = p.DVRDepth,
                    DVRLockDays = p.DVRLockDays,
                    DVRSpace = p.DVRSpace,
                    IsAdjustable = p.IsAdjustable,
                    IsDefault = p.IsDefault,
                    PreciseTrumbnailsDays = p.PreciseTrumbnailsDays,
                    IsDeleted = p.IsDeleted,
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during presets finding {exception}", ex);
                return null;
            }
        }

        public async Task<PresetResponseDTO?> ChangePreset(PresetRequestDTO dto, int presetId)
        {
            try
            {
                var preset = await db.Presets.FirstOrDefaultAsync(p => p.Id == presetId);

                if (preset != null)
                {
                    if (dto.Title != null) preset.Title = dto.Title;
                    if (dto.DVRDepth != null) preset.DVRDepth = dto.DVRDepth.Value;
                    if (dto.DVRLockDays != null) preset.DVRLockDays = dto.DVRLockDays.Value;
                    if (dto.DVRSpace != null) preset.DVRSpace = dto.DVRSpace.Value;
                    if (dto.IsAdjustable != null) preset.IsAdjustable = dto.IsAdjustable.Value;
                    if (dto.IsDefault != null) preset.IsDefault = dto.IsDefault.Value;
                    if (dto.PreciseTrumbnailsDays != null) preset.PreciseTrumbnailsDays = dto.PreciseTrumbnailsDays.Value;

                    db.Update(preset);
                }
                else
                {
                    preset = new PresetsEntity
                    {
                        Title = dto.Title ?? "",
                        DVRDepth = dto.DVRDepth ?? 0,
                        DVRLockDays = dto.DVRLockDays ?? 0,
                        DVRSpace = dto.DVRSpace ?? 0,
                        IsAdjustable = dto.IsAdjustable ?? true,
                        IsDefault = dto.IsDefault ?? false,
                        PreciseTrumbnailsDays = dto.PreciseTrumbnailsDays ?? 0,
                        IsDeleted = false,
                    };

                    db.Presets.Add(preset);
                }

                await db.SaveChangesAsync();

                var query = db.Presets
                    .Where(p => p.Id == presetId)
                    .AsQueryable();

                var result = await query.Select(p => new PresetResponseDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    DVRDepth = p.DVRDepth,
                    DVRLockDays = p.DVRLockDays,
                    DVRSpace = p.DVRSpace,
                    IsAdjustable = p.IsAdjustable,
                    IsDefault = p.IsDefault,
                    PreciseTrumbnailsDays = p.PreciseTrumbnailsDays,
                    IsDeleted = p.IsDeleted,
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during presets finding {exception}", ex);
                return null;
            }
        }

        public async Task<bool?> DeletePreset(int presetId)
        {
            try
            {
                var preset = await db.Presets.FindAsync(presetId);
                if (preset == null)
                {
                    return false;
                }

                db.Presets.Remove(preset);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during presets finding {exception}", ex);
                return null;
            }
        }
    }
}
