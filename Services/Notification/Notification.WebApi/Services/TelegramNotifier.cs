using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Notification.WebApi.Services;

public class TelegramNotifier : ITelegramNotifier
{
    private readonly TelegramSettings _settings;
    private readonly TelegramBotClient? _bot;
    private readonly ILogger<TelegramNotifier> _logger;

    public TelegramNotifier(IOptions<TelegramSettings> settings, ILogger<TelegramNotifier> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (!string.IsNullOrEmpty(_settings.BotToken))
        {
            _bot = new TelegramBotClient(_settings.BotToken);
            _logger.LogInformation("Telegram bot configured, chat: {ChatId}", _settings.ChatId);
        }
        else
        {
            _logger.LogWarning("Telegram bot token not configured. Messages will be logged only.");
        }
    }

    public async Task SendMessageAsync(string message)
    {
        _logger.LogInformation("Telegram: {Message}", message);

        if (_bot == null || string.IsNullOrEmpty(_settings.ChatId))
        {
            _logger.LogWarning("Telegram not configured, message only logged");
            return;
        }

        try
        {
            await _bot.SendMessage(_settings.ChatId, message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send Telegram message");
        }
    }
}
