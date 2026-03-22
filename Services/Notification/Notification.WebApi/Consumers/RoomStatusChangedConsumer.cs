using MassTransit;
using Notification.WebApi.Services;
using Shared.Contracts.Enums;
using Shared.Contracts.IntegrationEvents.Hotels;

namespace Notification.WebApi.Consumers;

public class RoomStatusChangedConsumer : IConsumer<RoomStatusChangedIntegrationEvent>
{
    private readonly ITelegramNotifier _telegram;
    private readonly ILogger<RoomStatusChangedConsumer> _logger;

    public RoomStatusChangedConsumer(ITelegramNotifier telegram, ILogger<RoomStatusChangedConsumer> logger)
    {
        _telegram = telegram;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RoomStatusChangedIntegrationEvent> context)
    {
        var evt = context.Message;

        _logger.LogInformation(
            "Room status changed: Room {RoomNumber} ({PrevStatus} -> {NewStatus})",
            evt.RoomNumber, evt.PreviousStatus, evt.NewStatus);

        // Notify when room goes out of service
        if (evt.NewStatus == RoomStatus.OutOfService)
        {
            var message = "<b>Номер выведен из эксплуатации</b>\n\n"
                        + $"Номер: <b>{evt.RoomNumber}</b>\n"
                        + $"Статус: {evt.PreviousStatus} -> <b>{evt.NewStatus}</b>\n"
                        + $"Время: {evt.Timestamp:dd.MM.yyyy HH:mm}";

            await _telegram.SendMessageAsync(message);
        }
    }
}
