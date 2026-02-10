using Admin.Application.Models;
using Admin.Application.Services;
using Admin.Infrastructure.Persistence;

namespace Admin.xUnitTests;

public sealed class SystemSettingsServiceTests
{
    [Fact]
    public async Task Upsert_Creates_Then_Updates()
    {
        var repository = new InMemorySystemSettingsRepository();
        var service = new SystemSettingsService(repository);

        var created = await service.Upsert("Reservation.HoldHours", "12", CancellationToken.None);
        Assert.Equal(SystemSettingUpsertResult.Created, created);

        var updated = await service.Upsert("Reservation.HoldHours", "24", CancellationToken.None);
        Assert.Equal(SystemSettingUpsertResult.Updated, updated);

        var setting = await service.GetByKey("Reservation.HoldHours", CancellationToken.None);
        Assert.NotNull(setting);
        Assert.Equal("24", setting.Value);
    }

    [Fact]
    public async Task Delete_ReturnsFalse_WhenMissing()
    {
        var repository = new InMemorySystemSettingsRepository();
        var service = new SystemSettingsService(repository);

        var deleted = await service.Delete("Not.Exist", CancellationToken.None);
        Assert.False(deleted);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("bad key")]
    [InlineData("bad/key")]
    public async Task GetByKey_Throws_OnInvalidKey(string key)
    {
        var repository = new InMemorySystemSettingsRepository();
        var service = new SystemSettingsService(repository);

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetByKey(key, CancellationToken.None));
    }
}

