using Microsoft.EntityFrameworkCore;
using Customers.Core.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Customers.Core.Data;

namespace Core.Data.PostgreSQL;


public class PostgreAppDbContext : DbContext
{
    // DbSet должен использовать класс Customer, а не CustomerDto
    public DbSet<Customer> Customers { get; set; }

    public PostgreAppDbContext(DbContextOptions<PostgreAppDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Создаем SEQUENCE для PostgreSQL
        modelBuilder.HasSequence<long>("user_id_seq")
            .StartsAt(1000000000L)
            .IncrementsBy(1)
            .HasMin(1000000000L);

        // Опционально: настройка модели
        modelBuilder.Entity<Customer>(entity =>
        {
            // Ключи
            entity.HasKey(e => e.Id);
            entity.HasAlternateKey(e => e.UserId);

            // Свойства с генерацией значений
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()"); // PostgreSQL 13+
                                                          // Или для старых версий: .HasDefaultValueSql("uuid_generate_v4()")

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasDefaultValueSql("nextval('\"user_id_seq\"'::regclass)");
            // Для Npgsql можно так: .HasDefaultValueSql("nextval('user_id_seq')")

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Preferences)
                .HasColumnType("jsonb") // Если храните как JSON
                .HasDefaultValueSql("'{}'::jsonb");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAddOrUpdate();
        });
    }
}