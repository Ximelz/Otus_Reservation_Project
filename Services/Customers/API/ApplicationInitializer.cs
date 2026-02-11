using System.Diagnostics;

namespace Customers.API
{
    public class ApplicationInitializer
    {
        public async Task InitializeApplicationAsync()
        {
            Console.WriteLine("Проверка зависимостей...");

            // Проверяем установлен ли Docker
            if (!await CheckDockerInstalledAsync())
            {
                Console.WriteLine("Docker не установлен. Установите Docker Desktop.");
                return;
            }

            // Проверяем запущен ли Docker
            if (!await CheckDockerRunningAsync())
            {
                Console.WriteLine("Docker не запущен. Запустите Docker Desktop.");
                return;
            }

            // Запускаем docker-compose
            await StartDockerComposeAsync();

            // Ждем пока сервисы станут доступны
            await WaitForServicesAsync();

            Console.WriteLine("Все сервисы запущены. Запускаем основное приложение...");
        }

        private async Task<bool> CheckDockerInstalledAsync()
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });

                if (process == null) return false;

                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckDockerRunningAsync()
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = "info",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });

                if (process == null) return false;

                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private async Task StartDockerComposeAsync()
        {
            var composeRunner = new DockerComposeService();
            await composeRunner.StartDockerComposeAsync();
        }

        private async Task WaitForServicesAsync()
        {
            // Пример: проверяем доступность сервисов
            // Например, проверяем доступность порта
            var services = new[]
            {
            new { Name = "Database", Host = "localhost", Port = 5432 },
            new { Name = "Redis", Host = "localhost", Port = 6379 }
        };

            foreach (var service in services)
            {
                Console.WriteLine($"Ожидание {service.Name}...");
                await WaitForPortAsync(service.Host, service.Port, TimeSpan.FromSeconds(30));
            }
        }

        private async Task WaitForPortAsync(string host, int port, TimeSpan timeout)
        {
            var startTime = DateTime.UtcNow;

            while (DateTime.UtcNow - startTime < timeout)
            {
                try
                {
                    using var client = new System.Net.Sockets.TcpClient();
                    await client.ConnectAsync(host, port);
                    Console.WriteLine($"{host}:{port} доступен");
                    return;
                }
                catch
                {
                    await Task.Delay(1000);
                }
            }

            throw new TimeoutException($"Сервис {host}:{port} не стал доступен за {timeout.TotalSeconds} секунд");
        }
    }
}
