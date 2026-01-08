using Hotels.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hotels.Infrastructure
{
    public class SqlDatabaseContext : DbContext
    {
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Country> Countries => Set<Country>();

        public SqlDatabaseContext()
        {
        }

        public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("hotels");

                entity.HasKey(h => h.Id)
                      .HasName("pk_hotels");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(p => p.Stars)
                      .HasColumnName("stars")
                      .HasColumnType("integer")
                      .HasDefaultValue(0);

                entity.Property(p => p.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");

                entity.Property(p => p.Address)
                      .HasColumnName("address")
                      .HasColumnType("text");

                entity.Property(p => p.Phone)
                      .HasColumnName("phone")
                      .HasColumnType("text");

                entity.Property(p => p.Email)
                      .HasColumnName("email")
                      .HasColumnType("text");

                entity.Property(p => p.CountryId)
                      .HasColumnName("country_id")
                      .HasColumnType("integer");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("rooms");

                entity.HasKey(h => h.Id)
                      .HasName("pk_rooms");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(p => p.Number)
                      .HasColumnName("number")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(p => p.Capacity)
                      .HasColumnName("capacity")
                      .HasColumnType("integer")
                      .HasDefaultValue(1);

                entity.Property(p => p.IsEnabled)
                      .HasColumnName("is_enabled")
                      .HasColumnType("boolean")
                      .HasDefaultValue(1);

                entity.Property(p => p.TypeId)
                      .HasColumnName("type_id")
                      .HasColumnType("bigint")
                      .HasDefaultValue(0);

                entity.HasIndex(r => r.HotelId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");

                entity.HasKey(h => h.Id)
                      .HasName("pk_countries");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("integer")
                      .ValueGeneratedNever();

                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .HasColumnType("text")
                      .IsRequired();
            });
            //modelBuilder.Entity<Hotel>().HasMany(h => h.Rooms).WithOne(r => r.Hotel);
            //modelBuilder.Entity<Room>().HasOne(r => r.Hotel);
            //modelBuilder.Entity<Hotel>().HasOne(h => h.Country);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            PgDbContextOptions pgOptions = new();
            optionsBuilder.UseNpgsql(pgOptions.GetConnectionString());

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
