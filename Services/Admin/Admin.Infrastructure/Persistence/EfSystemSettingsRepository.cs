using Admin.Application.Abstractions;
using Admin.Application.Models;
using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Persistence;

public sealed class EfSystemSettingsRepository : ISystemSettingsRepository
{
    private readonly AdminDbContext _db;

    public EfSystemSettingsRepository(AdminDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SystemSetting>> GetAll(CancellationToken cancellationToken)
    {
        return await _db.SystemSettings
            .AsNoTracking()
            .OrderBy(x => x.Key)
            .ToListAsync(cancellationToken);
    }

    public async Task<SystemSetting?> GetByKey(string key, CancellationToken cancellationToken)
    {
        return await _db.SystemSettings
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Key == key, cancellationToken);
    }

    public async Task<SystemSettingUpsertResult> Upsert(string key, string value, CancellationToken cancellationToken)
    {
        var existing = await _db.SystemSettings.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);
        if (existing is null)
        {
            _db.SystemSettings.Add(new SystemSetting
            {
                Key = key,
                Value = value,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            await _db.SaveChangesAsync(cancellationToken);
            return SystemSettingUpsertResult.Created;
        }

        existing.Value = value;
        existing.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
        return SystemSettingUpsertResult.Updated;
    }

    public async Task<bool> Delete(string key, CancellationToken cancellationToken)
    {
        var existing = await _db.SystemSettings.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        _db.SystemSettings.Remove(existing);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}

