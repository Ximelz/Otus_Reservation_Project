using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Core.Interfaces
{
    public interface IDBEFClass
    {
        public string GetConnectionString();
        public void EnsureCreatedDatabase();
        public Action<DbContextOptionsBuilder> ConfigureDbContextOptions();
    }
}
