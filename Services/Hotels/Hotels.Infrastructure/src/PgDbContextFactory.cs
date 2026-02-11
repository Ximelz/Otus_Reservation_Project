using Microsoft.EntityFrameworkCore.Design;

namespace Hotels.Infrastructure
{
    public class PgDbContextFactory : IDesignTimeDbContextFactory<PgDbContext>
    {
        public PgDbContext CreateDbContext(string[] args)
        {
            PgDbContextOptions pgOptions = new();

            return new PgDbContext(pgOptions);
        }
    }
}
