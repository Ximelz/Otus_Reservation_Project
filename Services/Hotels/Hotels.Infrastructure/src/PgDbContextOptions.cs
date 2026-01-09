using Hotels.Setup;
using Microsoft.EntityFrameworkCore;


namespace Hotels.Infrastructure
{
    public class PgDbContextOptions
    {
        public string UserId 
        { 
            get => _pgSettings.UserId; 
            set => _pgSettings.UserId = value; 
        }
        public string Password
        {
            get => _pgSettings.Password;
            set => _pgSettings.Password = value;
        }
        public string Host
        {
            get => _pgSettings.Host;
            set => _pgSettings.Host = value;
        }
        public string Port
        {
            get => _pgSettings.Port;
            set => _pgSettings.Port = value;
        }
        public string DatabaseName
        {
            get => _pgSettings.DatabaseName;
            set => _pgSettings.DatabaseName = value;
        }

        private PgSettingsManager _pgSettings = new();

        public PgDbContextOptions()
        {
            _pgSettings.ReadSettings();
        }

        public DbContextOptions<SqlDatabaseContext> GetOptions()
        {
            return GetOptionsBuilder().Options;
        }
         
        public DbContextOptionsBuilder<SqlDatabaseContext> GetOptionsBuilder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlDatabaseContext>();

            return optionsBuilder.UseNpgsql(GetConnectionString());
        }

        public string GetConnectionString()
        {
            return $"User ID={UserId};Password={Password};Host={Host};Port={Port};Database={DatabaseName}";
        }
    }
}
