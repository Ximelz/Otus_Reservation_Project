using Admin.Domain.Entities;
using Admin.Application.Models;

namespace Admin.Application.Abstractions;

public interface ISystemSettingsRepository
{
    Task<IReadOnlyList<SystemSetting>> GetAll(CancellationToken cancellationToken);
    Task<SystemSetting?> GetByKey(string key, CancellationToken cancellationToken);
    Task<SystemSettingUpsertResult> Upsert(string key, string value, CancellationToken cancellationToken);
    Task<bool> Delete(string key, CancellationToken cancellationToken);
}
