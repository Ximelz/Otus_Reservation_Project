using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class DbContextFactory : IDbContextFactory<ReservDbContext>
    {
        public DbContextFactory(string dbConn) => this.dbConn = dbConn;
        private readonly string dbConn;
        public ReservDbContext CreateDbContext() => new ReservDbContext(dbConn);
    }
}
