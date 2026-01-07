using System.Text;

namespace Hotels.PgSetup
{
    public class PgSettingsManager
    {
        public string UserId { get; set; } = "postgres";
        public string Password { get; set; } = "";
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5432;
        public string DatabaseName { get; set; } = "Hotels";

        private static string ENV_VAR_NAME = "Otus-Reservation-hotels";
        private static string ENV_VARS_SEPARATOR = "#$%";
        private static string ENV_VAR_SEPARATOR = "=";
        private Dictionary<string, string> _settings = new();


        public PgSettingsManager()
        {
            SetDefaultSettings();
        }

        private void SetDefaultSettings()
        {
            _settings.Clear();

            _settings["userid"] = "postgres";
            _settings["password"] = "";
            _settings["host"] = "127.0.0.1";
            _settings["port"] = "5432";
            _settings["databasename"] = "hotels";
        }


        public void SetUserId(string userId)
        {
            _settings["userid"] = userId;
        }

        public void SetHost(string host)
        {
            _settings["host"] = host;
        }

        public void SetPort(string port)
        {
            _settings["port"] = port;
        }

        public void SetPassword(string password)
        {
            _settings["password"] = password;
        }

        public void SetDatabaseName(string databaseName)
        {
            _settings["databasename"] = databaseName;
        }

        public void ReadSettings()
        {
            _settings.Clear();

            string? settings = Environment.GetEnvironmentVariable(ENV_VAR_NAME, EnvironmentVariableTarget.User);
            
            if (string.IsNullOrEmpty(settings))
            {
                SetDefaultSettings();

                Console.WriteLine("Hotels' service: PostgreSQL settings is empty.");

                return;
            }

            foreach (var setting in settings.Split(ENV_VARS_SEPARATOR))
            {
                string[] vals = setting.Split(ENV_VAR_SEPARATOR);

                if (vals.Length == 0)
                {
                    continue;
                }
                else if (vals.Length != 2)
                {
                    Console.WriteLine($"Hotels' service: wrong format of PostgreSQL setting '{setting}'");
                    continue;
                }

                _settings[vals[0]] = vals[1];
            }
        }

        public void WriteSettings()
        {
            StringBuilder settings = new();

            foreach (var i in _settings)
            {
                if (settings.Length > 0)
                {
                    settings.Append(ENV_VARS_SEPARATOR);
                }

                settings.Append(i.Key);
                settings.Append(ENV_VAR_SEPARATOR);
                settings.Append(i.Value);
            }

            Environment.SetEnvironmentVariable(ENV_VAR_NAME, settings.ToString(), EnvironmentVariableTarget.User);
        }

        override public string ToString()
        {
            StringBuilder settings = new();

            foreach (var i in _settings)
            {
                settings.Append(i.Key);
                settings.Append(ENV_VAR_SEPARATOR);
                settings.Append(i.Value);
                settings.AppendLine();
            }

            return settings.ToString();
        }
    }
}
