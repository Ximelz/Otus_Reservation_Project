using Microsoft.EntityFrameworkCore;

namespace ReservService
{
    public class ReservDbContext : DbContext
    {
        public ReservDbContext(string dbConn) : base()
        {
            this.dbConn = dbConn;
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            this.SaveChanges();
        }
        private readonly string dbConn;
        public DbSet<ReserveModel> Reserves => Set<ReserveModel>();
        public DbSet<HotelReserveModel> Hotels => Set<HotelReserveModel>();
        public DbSet<PersonReserveModel> Users => Set<PersonReserveModel>();
        public DbSet<RoomReserveModel> Rooms => Set<RoomReserveModel>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(dbConn);
    }
}
