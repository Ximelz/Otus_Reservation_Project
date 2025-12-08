using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;


namespace Hotels.Infrastructure
{
    public class PgDbContextOptions
    {
        private string _configurationFilePath = "";

        public string UserId { get; set; } = "postgres";
        public string Password { get; set; } = "";
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5432;
        public string DatabaseName = "Reservation";

        public PgDbContextOptions(string configurationFilePath)
        {
            _configurationFilePath = configurationFilePath;

            ReadOptionsFromDbJson();
        }

        public DbContextOptions<SqlDatabaseContext> GetOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlDatabaseContext>();

            return optionsBuilder.UseNpgsql(GetConnectionString()).Options;
        }

        private string GetConnectionString()
        {
            return $"User ID={UserId};Password={Password};Host={Host};Port={Port};Database={DatabaseName}";
        }

        private void ReadOptionsFromDbJson()
        {
            if (!File.Exists(_configurationFilePath))
            {
                throw new FileNotFoundException("Файл конфигурации не найден", _configurationFilePath);
            }

            using (StreamReader r = new StreamReader(_configurationFilePath))
            {
                string jsonString = r.ReadToEnd();
                JsonNode rootNode = JsonNode.Parse(jsonString)!;
                JsonNode connectionNode = rootNode!["ConnectionData"]!;

                if (connectionNode == null)
                {
                    throw new Exception($"""
                        В файле конфигурации подключения к БД отсутствует узел ConnectionData.
                        Файл конфигурации: {_configurationFilePath}
                        """);
                }

                var valueNode = connectionNode!["UserId"]!;
                if (valueNode == null)
                {
                    throw new Exception("В файле конфигурации подключения к БД отсутствует значение id пользователя (UserId)");
                }

                UserId = valueNode.ToString();

                valueNode = connectionNode!["Password"]!;
                if (valueNode == null)
                {
                    throw new Exception("В файле конфигурации подключения к БД отсутствует значение пароля (Password)");
                }

                Password = valueNode.ToString();

                valueNode = connectionNode!["Host"]!;
                if (valueNode == null)
                {
                    throw new Exception("В файле конфигурации подключения к БД отсутствует значение хоста (Host)");
                }

                Host = valueNode.ToString();

                valueNode = connectionNode!["Port"]!;
                if (valueNode == null)
                {
                    throw new Exception("В файле конфигурации подключения к БД отсутствует значение порта (Port)");
                }

                int port = 0;
                if (!int.TryParse(valueNode.ToString(), out port))
                {
                    throw new Exception("В файле конфигурации подключения к БД задано не числовое значение порта (Port): " + valueNode.ToString());
                }

                Port = port;

                valueNode = connectionNode!["Database"]!;
                if (valueNode == null)
                {
                    throw new Exception("В файле конфигурации подключения к БД отсутствует значение названия БД (Database)");
                }

                DatabaseName = valueNode.ToString();
            }
        }
    }
}
