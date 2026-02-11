using Hotels.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hotels.Infrastructure
{
    public class PgDbContext : DbContext
    {
        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<RatePlan> RatePlans => Set<RatePlan>();

        private PgDbContextOptions _pgOptions;

        public PgDbContext(PgDbContextOptions options) : base(options.GetOptions())
        {
            _pgOptions = options;
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

                entity.Property(p => p.IsEnabled)
                      .HasColumnName("is_enabled")
                      .HasColumnType("boolean")
                      .HasDefaultValue(1);

                entity.Property(p => p.TypeId)
                      .HasColumnName("type_id")
                      .HasColumnType("uuid");

                entity.HasIndex(r => r.HotelId);
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("room_types");

                entity.HasKey(h => h.Id)
                      .HasName("pk_room_types");

                entity.Property(p => p.Id)
                      .IsRequired()
                      .HasColumnName("id")
                      .HasColumnType("uuid");

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(p => p.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");

                entity.Property(p => p.Capacity)
                      .HasColumnName("capacity")
                      .HasColumnType("integer")
                      .HasDefaultValue(1);

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

            modelBuilder.Entity<RatePlan>(entity =>
            {
                entity.ToTable("rate_plans");

                // p - property
                entity.HasKey(p => p.Id)
                      .HasName("pk_rate_plans");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(p => p.RoomTypeId)
                      .HasColumnName("room_type_id")
                      .HasColumnType("uuid");

                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(p => p.Price)
                      .HasColumnName("price")
                      .HasColumnType("numeric")
                      .IsRequired();

                entity.HasIndex(r => r.HotelId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _pgOptions.ConfigureOptionsBuilder(optionsBuilder);

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
