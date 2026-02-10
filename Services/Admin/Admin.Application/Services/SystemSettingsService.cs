using System.Text.RegularExpressions;
using Admin.Application.Abstractions;
using Admin.Application.Models;
using Admin.Domain.Entities;

namespace Admin.Application.Services;

public sealed class SystemSettingsService
{
    private static readonly Regex KeyRegex = new("^[A-Za-z0-9][A-Za-z0-9._-]{0,127}$", RegexOptions.Compiled);
    private readonly ISystemSettingsRepository _repository;

    public SystemSettingsService(ISystemSettingsRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<SystemSetting>> GetAll(CancellationToken cancellationToken)
        => _repository.GetAll(cancellationToken);

    public Task<SystemSetting?> GetByKey(string key, CancellationToken cancellationToken)
    {
        ValidateKey(key);
        return _repository.GetByKey(key, cancellationToken);
    }

    public Task<SystemSettingUpsertResult> Upsert(string key, string value, CancellationToken cancellationToken)
    {
        ValidateKey(key);
        if (value is null)
        {
            throw new ArgumentException("Value cannot be null.", nameof(value));
        }

        return _repository.Upsert(key, value, cancellationToken);
    }

    public Task<bool> Delete(string key, CancellationToken cancellationToken)
    {
        ValidateKey(key);
        return _repository.Delete(key, cancellationToken);
    }

    private static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be empty.", nameof(key));
        }

        if (!KeyRegex.IsMatch(key))
        {
            throw new ArgumentException("Invalid key format. Allowed: A-Za-z0-9 plus ._- (max 128).", nameof(key));
        }
    }
}

