using System.Text;

namespace Hotels.Setup
{
    /// Настройки подключения к БД PostgreSQL. 
    /// Хранятся в реестре: HKEY_CURRENT_USER/Environment.
    /// Название переменной: Otus-Reservation-hotels
    /// Значение - строка с набором настроек:
    /// - userId
    /// - host
    /// - port
    /// - password
    /// - databaseName
    /// 
    /// Например:userId=postgre;host=127.0.0.1;port=5432;password=2345;databaseName=hotels

    public class PgSettingsManager
    {
        private Dictionary<string, string> _settings = new();

        public string UserId
        {
            get => _settings["userId"];
            set => _settings["userId"] = value;
        }
        public string Password
        {
            get => _settings["password"];
            set => _settings["password"] = value;
        }
        public string Host
        {
            get => _settings["host"];
            set => _settings["host"] = value;
        }
        public string Port
        {
            get => _settings["port"];
            set => _settings["port"] = value;
        }
        public string DatabaseName
        {
            get => _settings["databaseName"];
            set => _settings["databaseName"] = value;
        }

        private static string ENV_VAR_NAME = "Otus-Reservation-hotels";
        private static string ENV_VARS_SEPARATOR = "#$%";
        private static string ENV_VAR_SEPARATOR = "=";
        

        public PgSettingsManager()
        {
            SetDefaultSettings();
        }

        private void SetDefaultSettings()
        {
            _settings.Clear();
            _settings["userId"] = "postgres";
            _settings["password"] = "12345";
            _settings["host"] = "localhost";
            _settings["port"] = "15433";
            _settings["databaseName"] = "HotelDB";
            //_settings["userId"] = "postgres";
            //_settings["password"] = "12345";
            //_settings["host"] = "127.0.0.1";
            //_settings["port"] = "5432";
            //_settings["databaseName"] = "hotels";
        }

        public bool ReadSettings()
        {
            _settings.Clear();

            string? settings = Environment.GetEnvironmentVariable(ENV_VAR_NAME, EnvironmentVariableTarget.User);
            
            if (string.IsNullOrEmpty(settings))
            {
                SetDefaultSettings();

                Console.WriteLine("Hotels' service: PostgreSQL settings is empty.");

                return false;
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

            return true;
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
