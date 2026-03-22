namespace Notification.WebApi.Services;

public interface ITelegramNotifier
{
    Task SendMessageAsync(string message);
}
