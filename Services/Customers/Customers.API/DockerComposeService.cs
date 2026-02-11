using System.Diagnostics;

namespace Customers.API
{
    public class DockerComposeService
    {
        public async Task StartDockerComposeAsync(string composeFile = "docker-compose.yml")
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                string fullPath = Path.GetFullPath(composeFile);
                string workingDir = Path.GetDirectoryName(fullPath) ?? Directory.GetCurrentDirectory();

                var processInfo = new ProcessStartInfo
                {
                    FileName = "docker-compose",
                    Arguments = "up -d",
                    WorkingDirectory = workingDir,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = new Process();
                process.StartInfo = processInfo;

                // Обработка вывода
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"[Docker] {e.Data}");
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"[Docker Error] {e.Data}");
                };

                process.Exited += (sender, e) =>
                {
                    tcs.TrySetResult(true);
                };

                process.EnableRaisingEvents = true;

                if (process.Start())
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await tcs.Task;
                    Console.WriteLine("Docker-compose успешно запущен");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                tcs.TrySetException(ex);
            }
        }
    }
}
