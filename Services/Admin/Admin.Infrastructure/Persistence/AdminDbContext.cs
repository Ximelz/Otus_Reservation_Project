using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Persistence;

public sealed class AdminDbContext : DbContext
{
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SystemSetting>(b =>
        {
            b.HasKey(x => x.Key);
            b.Property(x => x.Key).HasMaxLength(128).IsRequired();
            b.Property(x => x.Value).HasMaxLength(4000).IsRequired();
            b.Property(x => x.UpdatedAt).IsRequired();
        });
    }
}

