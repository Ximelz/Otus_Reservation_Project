using Hotels.Domain.Entities;
using Hotels.Domain.Enums;
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
        public DbSet<SeasonPrice> SeasonPrices => Set<SeasonPrice>();
        public DbSet<HotelAmenity> HotelAmenities => Set<HotelAmenity>();
        public DbSet<HotelPolicy> HotelPolicies => Set<HotelPolicy>();
        public DbSet<RecentActivityLog> RecentActivityLogs => Set<RecentActivityLog>();

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

                entity.Property(p => p.Slug)
                      .HasColumnName("slug")
                      .HasColumnType("varchar(128)")
                      .IsRequired();

                entity.Property(p => p.Stars)
                      .HasColumnName("stars")
                      .HasColumnType("integer")
                      .HasDefaultValue(0);

                entity.Property(p => p.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");

                entity.Property(p => p.City)
                      .HasColumnName("city")
                      .HasColumnType("varchar(256)");

                entity.Property(p => p.Address)
                      .HasColumnName("address")
                      .HasColumnType("text");

                entity.Property(p => p.CountryId)
                      .HasColumnName("country_id")
                      .HasColumnType("integer");

                entity.Property(p => p.Timezone)
                      .HasColumnName("timezone")
                      .HasColumnType("varchar(64)")
                      .HasDefaultValue("Europe/Moscow");

                entity.Property(p => p.CheckInTime)
                      .HasColumnName("check_in_time")
                      .HasColumnType("time");

                entity.Property(p => p.CheckOutTime)
                      .HasColumnName("check_out_time")
                      .HasColumnType("time");

                entity.Property(p => p.Phone)
                      .HasColumnName("phone")
                      .HasColumnType("text");

                entity.Property(p => p.Email)
                      .HasColumnName("email")
                      .HasColumnType("text");

                entity.Property(p => p.IsActive)
                      .HasColumnName("is_active")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.Property(p => p.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.Property(p => p.CreatedBy)
                      .HasColumnName("created_by")
                      .HasColumnType("varchar(256)");

                entity.Property(p => p.UpdatedBy)
                      .HasColumnName("updated_by")
                      .HasColumnType("varchar(256)");

                entity.HasIndex(h => h.Slug).IsUnique().HasDatabaseName("ix_hotels_slug");
                entity.HasIndex(h => h.IsActive).HasDatabaseName("ix_hotels_is_active");

                entity.HasMany(h => h.Amenities).WithOne(a => a.Hotel).HasForeignKey(a => a.HotelId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(h => h.RoomTypes).WithOne(rt => rt.Hotel).HasForeignKey(rt => rt.HotelId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(h => h.Rooms).WithOne(r => r.Hotel).HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(h => h.RatePlans).WithOne(rp => rp.Hotel).HasForeignKey(rp => rp.HotelId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(h => h.Policy).WithOne(p => p.Hotel).HasForeignKey<HotelPolicy>(p => p.HotelId).OnDelete(DeleteBehavior.Cascade);
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

                entity.Property(p => p.TypeId)
                      .HasColumnName("type_id")
                      .HasColumnType("uuid");

                entity.Property(p => p.Floor)
                      .HasColumnName("floor")
                      .HasColumnType("integer")
                      .HasDefaultValue(1);

                entity.Property(p => p.Status)
                      .HasColumnName("status")
                      .HasColumnType("integer")
                      .HasDefaultValue(RoomStatus.Available);

                entity.Property(p => p.HousekeepingStatus)
                      .HasColumnName("housekeeping_status")
                      .HasColumnType("integer")
                      .HasDefaultValue(HousekeepingStatus.Clean);

                entity.Property(p => p.ViewType)
                      .HasColumnName("view_type")
                      .HasColumnType("varchar(64)");

                entity.Property(p => p.Notes)
                      .HasColumnName("notes")
                      .HasColumnType("text");

                entity.Property(p => p.IsActive)
                      .HasColumnName("is_active")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.Property(p => p.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.HasIndex(r => r.HotelId).HasDatabaseName("ix_rooms_hotel_id");
                entity.HasIndex(r => new { r.HotelId, r.Number }).IsUnique().HasDatabaseName("ix_rooms_hotel_number");

                entity.HasOne(r => r.RoomType).WithMany(rt => rt.Rooms).HasForeignKey(r => r.TypeId).OnDelete(DeleteBehavior.Restrict);
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

                entity.Property(p => p.Code)
                      .HasColumnName("code")
                      .HasColumnType("varchar(32)");

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

                entity.Property(p => p.CapacityAdults)
                      .HasColumnName("capacity_adults")
                      .HasColumnType("integer")
                      .HasDefaultValue(2);

                entity.Property(p => p.CapacityChildren)
                      .HasColumnName("capacity_children")
                      .HasColumnType("integer")
                      .HasDefaultValue(0);

                entity.Property(p => p.BedConfiguration)
                      .HasColumnName("bed_configuration")
                      .HasColumnType("varchar(128)");

                entity.Property(p => p.BaseAreaSqm)
                      .HasColumnName("base_area_sqm")
                      .HasColumnType("numeric")
                      .HasDefaultValue(0m);

                entity.Property(p => p.IsActive)
                      .HasColumnName("is_active")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.Property(p => p.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.HasIndex(r => r.HotelId).HasDatabaseName("ix_room_types_hotel_id");
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

                entity.Property(p => p.Code)
                      .HasColumnName("code")
                      .HasColumnType("varchar(32)");

                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(p => p.BasePrice)
                      .HasColumnName("base_price")
                      .HasColumnType("numeric")
                      .IsRequired();

                entity.Property(p => p.Currency)
                      .HasColumnName("currency")
                      .HasColumnType("varchar(3)")
                      .HasDefaultValue("RUB");

                entity.Property(p => p.CancellationPolicyType)
                      .HasColumnName("cancellation_policy_type")
                      .HasColumnType("integer")
                      .HasDefaultValue(CancellationPolicyType.Free);

                entity.Property(p => p.BreakfastIncluded)
                      .HasColumnName("breakfast_included")
                      .HasColumnType("boolean")
                      .HasDefaultValue(false);

                entity.Property(p => p.PrepaymentRequired)
                      .HasColumnName("prepayment_required")
                      .HasColumnType("boolean")
                      .HasDefaultValue(false);

                entity.Property(p => p.IsDefault)
                      .HasColumnName("is_default")
                      .HasColumnType("boolean")
                      .HasDefaultValue(false);

                entity.Property(p => p.IsActive)
                      .HasColumnName("is_active")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.Property(p => p.CreatedAt)
                      .HasColumnName("created_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.Property(p => p.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.HasIndex(r => r.HotelId).HasDatabaseName("ix_rate_plans_hotel_id");

                entity.HasOne(rp => rp.RoomType).WithMany(rt => rt.RatePlans).HasForeignKey(rp => rp.RoomTypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SeasonPrice>(entity =>
            {
                entity.ToTable("season_prices");

                entity.HasKey(h => h.Id)
                      .HasName("pk_season_prices");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.RoomTypeId)
                      .HasColumnName("room_type_id")
                      .HasColumnType("uuid");

                entity.Property(p => p.Multiplier)
                      .HasColumnName("multiplier")
                      .HasColumnType("numeric")
                      .IsRequired();

                entity.Property(p => p.DateFrom)
                      .HasColumnName("date_from")
                      .HasColumnType("date")
                      .IsRequired();

                entity.Property(p => p.DateTo)
                      .HasColumnName("date_to")
                      .HasColumnType("date")
                      .IsRequired();

                entity.HasIndex(r => r.RoomTypeId).HasDatabaseName("ix_season_prices_room_type_id");
            });

            modelBuilder.Entity<HotelAmenity>(entity =>
            {
                entity.ToTable("hotel_amenities");

                entity.HasKey(a => a.Id).HasName("pk_hotel_amenities");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(p => p.AmenityType)
                      .HasColumnName("amenity_type")
                      .HasColumnType("integer")
                      .IsRequired();

                entity.Property(p => p.CustomName)
                      .HasColumnName("custom_name")
                      .HasColumnType("varchar(128)");

                entity.Property(p => p.IsAvailable)
                      .HasColumnName("is_available")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.HasIndex(a => a.HotelId).HasDatabaseName("ix_hotel_amenities_hotel_id");
            });

            modelBuilder.Entity<HotelPolicy>(entity =>
            {
                entity.ToTable("hotel_policies");

                entity.HasKey(p => p.Id).HasName("pk_hotel_policies");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid")
                      .IsRequired();

                entity.Property(p => p.CheckInTime)
                      .HasColumnName("check_in_time")
                      .HasColumnType("time");

                entity.Property(p => p.CheckOutTime)
                      .HasColumnName("check_out_time")
                      .HasColumnType("time");

                entity.Property(p => p.EarlyCheckInNote)
                      .HasColumnName("early_check_in_note")
                      .HasColumnType("text");

                entity.Property(p => p.LateCheckOutNote)
                      .HasColumnName("late_check_out_note")
                      .HasColumnType("text");

                entity.Property(p => p.CancellationPolicyText)
                      .HasColumnName("cancellation_policy_text")
                      .HasColumnType("text");

                entity.Property(p => p.ConfirmationPendingEnabled)
                      .HasColumnName("confirmation_pending_enabled")
                      .HasColumnType("boolean")
                      .HasDefaultValue(false);

                entity.Property(p => p.AutoConfirmRules)
                      .HasColumnName("auto_confirm_rules")
                      .HasColumnType("boolean")
                      .HasDefaultValue(false);

                entity.Property(p => p.TermsAndConditionsText)
                      .HasColumnName("terms_and_conditions_text")
                      .HasColumnType("text");

                entity.Property(p => p.ContactInstructions)
                      .HasColumnName("contact_instructions")
                      .HasColumnType("text");

                entity.Property(p => p.UpdatedAt)
                      .HasColumnName("updated_at")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.HasIndex(p => p.HotelId).IsUnique().HasDatabaseName("ix_hotel_policies_hotel_id");
            });

            modelBuilder.Entity<RecentActivityLog>(entity =>
            {
                entity.ToTable("recent_activity_logs");

                entity.HasKey(a => a.Id).HasName("pk_recent_activity_logs");

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasColumnType("uuid")
                      .ValueGeneratedNever();

                entity.Property(p => p.HotelId)
                      .HasColumnName("hotel_id")
                      .HasColumnType("uuid");

                entity.Property(p => p.ActivityType)
                      .HasColumnName("activity_type")
                      .HasColumnType("varchar(64)")
                      .IsRequired();

                entity.Property(p => p.Description)
                      .HasColumnName("description")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(p => p.EntityType)
                      .HasColumnName("entity_type")
                      .HasColumnType("varchar(64)");

                entity.Property(p => p.EntityId)
                      .HasColumnName("entity_id")
                      .HasColumnType("uuid");

                entity.Property(p => p.Timestamp)
                      .HasColumnName("timestamp")
                      .HasColumnType("timestamptz")
                      .HasDefaultValueSql("now()");

                entity.Property(p => p.PerformedBy)
                      .HasColumnName("performed_by")
                      .HasColumnType("varchar(256)");

                entity.HasIndex(a => a.HotelId).HasDatabaseName("ix_recent_activity_logs_hotel_id");
                entity.HasIndex(a => a.Timestamp).HasDatabaseName("ix_recent_activity_logs_timestamp");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _pgOptions.ConfigureOptionsBuilder(optionsBuilder);

            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
