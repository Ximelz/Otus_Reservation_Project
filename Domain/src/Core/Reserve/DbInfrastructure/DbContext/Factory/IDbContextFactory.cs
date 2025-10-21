using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public interface IDbContextFactory<TDbContext> where TDbContext : DbContext 
    {
        public TDbContext CreateDbContext();
    }
}
