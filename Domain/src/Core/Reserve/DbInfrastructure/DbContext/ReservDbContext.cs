using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(dbConn);
    }
}
