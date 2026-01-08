using Hotels.Setup;

namespace Hotels.PgSetup
{
    /// <summary>
    /// 
    /// Установка необходимых настроек для подключения к БД на PostgreSQL.
    /// 
    /// Настройки хранятся в реестре: HKEY_CURRENT_USER/Environment.
    /// Название переменной: Otus-Reservation-hotels
    /// Значение - строка с набором настроек:
    /// - userId
    /// - host
    /// - port
    /// - password
    /// - databaseName
    /// 
    /// Например:userId=postgre;host=127.0.0.1;port=5432;password=2345;databaseName=hotels
    /// </summary>

    internal class Program
    {
        static void Main(string[] args)
        {
            PgSettingsManager pgset = new();

            while (true)
            {
                Console.WriteLine("Enter an item from the list below.");
                Console.WriteLine("1. Set user id");
                Console.WriteLine("2. Set host");
                Console.WriteLine("3. Set port");
                Console.WriteLine("4. Set password");
                Console.WriteLine("5. Set database name");
                Console.WriteLine("--------------------");
                Console.WriteLine("6. Read settings");
                Console.WriteLine("7. Save settings");
                Console.WriteLine("8. Show settings");
                Console.WriteLine("--------------------");
                Console.WriteLine("0. Quit");
                Console.WriteLine();

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                Console.Clear();

                switch (keyInfo.KeyChar)
                {
                    case '1':
                        {
                            Console.WriteLine("Enter a user id:");
                            string? userid = Console.ReadLine();

                            if (string.IsNullOrEmpty(userid))
                            {
                                Console.WriteLine("The string is empty. Enter a non-empty text.");
                                Console.ReadKey();

                                continue;
                            }

                            pgset.UserId = userid;
                            break;
                        }
                    case '2':
                        {
                            Console.WriteLine("Enter a host:");
                            string? host = Console.ReadLine();

                            if (string.IsNullOrEmpty(host))
                            {
                                Console.WriteLine("The string is empty. Enter a non-empty text.");
                                Console.ReadKey();

                                continue;
                            }

                            pgset.Host = host;
                            break;
                        }
                    case '3':
                        {
                            Console.WriteLine("Enter a port:");
                            string? sport = Console.ReadLine();

                            if (string.IsNullOrEmpty(sport))
                            {
                                Console.WriteLine("The string is empty. Enter a non-empty text.");
                                Console.ReadKey();

                                continue;
                            }

                            int port = 0;
                            if (!int.TryParse(sport, out port))
                            {
                                Console.WriteLine("The port is non-integer. Enter an integer value.");
                                Console.ReadKey();

                                continue;
                            }

                            pgset.Port = port.ToString();
                            break;
                        }
                    case '4':
                        {
                            Console.WriteLine("Enter a password:");
                            string? psw = Console.ReadLine();

                            if (string.IsNullOrEmpty(psw))
                            {
                                pgset.Password = string.Empty;
                            }
                            else
                            {
                                pgset.Password = psw;
                            }
                            
                            break;
                        }
                    case '5':
                        {
                            Console.WriteLine("Enter a database name:");
                            string? name = Console.ReadLine();

                            if (string.IsNullOrEmpty(name))
                            {
                                Console.WriteLine("The string is empty. Enter a non-empty text.");
                                Console.ReadKey();

                                continue;
                            }

                            pgset.DatabaseName = name;

                            break;
                        }
                    case '8':
                        {
                            Console.WriteLine(pgset.ToString());
                            Console.ReadLine();

                            break;
                        }
                    case '7':
                        {
                            pgset.WriteSettings();
                            Console.WriteLine("Settings are written");
                            Console.ReadLine();

                            break;
                        }
                    case '6':
                        {
                            pgset.ReadSettings();
                            Console.WriteLine("Readed settings are:");
                            Console.WriteLine(pgset.ToString());
                            Console.ReadLine();

                            break;
                        }
                    default:
                        {
                            return;
                        }
                }

                Console.Clear();
            }
        }
    }
}
