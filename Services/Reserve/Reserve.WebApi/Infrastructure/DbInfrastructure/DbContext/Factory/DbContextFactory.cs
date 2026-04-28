using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class DbContextFactory : IDbContextFactory<ReservDbContext>
    {
        public DbContextFactory(IConnectString dbConn) => this.dbConn = dbConn;
        private readonly IConnectString dbConn;
        public ReservDbContext CreateDbContext() => new ReservDbContext(dbConn);
    }
}
