using Admin.Application.Abstractions;
using Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Infrastructure.DependencyInjection;

public static class AdminInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddAdminPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var storageMode = configuration["Admin:Storage:Mode"] ?? "InMemory";
        if (string.Equals(storageMode, "Postgres", StringComparison.OrdinalIgnoreCase))
        {
            var connectionString = configuration["Admin:Database:ConnectionString"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Admin:Database:ConnectionString is required for Postgres storage mode.");
            }

            services.AddDbContext<AdminDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<ISystemSettingsRepository, EfSystemSettingsRepository>();
            services.AddHostedService<AdminDbInitializerHostedService>();

            return services;
        }

        services.AddSingleton<ISystemSettingsRepository, InMemorySystemSettingsRepository>();
        return services;
    }
}

