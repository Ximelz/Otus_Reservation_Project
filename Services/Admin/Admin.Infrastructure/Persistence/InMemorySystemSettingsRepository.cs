using System.Collections.Concurrent;
using Admin.Application.Abstractions;
using Admin.Application.Models;
using Admin.Domain.Entities;

namespace Admin.Infrastructure.Persistence;

public sealed class InMemorySystemSettingsRepository : ISystemSettingsRepository
{
    private readonly ConcurrentDictionary<string, SystemSetting> _storage = new(StringComparer.Ordinal);

    public Task<IReadOnlyList<SystemSetting>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<SystemSetting>>(_storage.Values.OrderBy(x => x.Key).ToList());
    }

    public Task<SystemSetting?> GetByKey(string key, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _storage.TryGetValue(key, out var value);
        return Task.FromResult<SystemSetting?>(value);
    }

    public Task<SystemSettingUpsertResult> Upsert(string key, string value, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var now = DateTimeOffset.UtcNow;
        var created = false;

        _storage.AddOrUpdate(
            key,
            _ =>
            {
                created = true;
                return new SystemSetting { Key = key, Value = value, UpdatedAt = now };
            },
            (_, existing) =>
            {
                existing.Value = value;
                existing.UpdatedAt = now;
                return existing;
            });

        return Task.FromResult(created ? SystemSettingUpsertResult.Created : SystemSettingUpsertResult.Updated);
    }

    public Task<bool> Delete(string key, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_storage.TryRemove(key, out _));
    }
}

