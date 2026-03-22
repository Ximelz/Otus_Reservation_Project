using MassTransit;
using Notification.WebApi.Services;
using Shared.Contracts.Enums;
using Shared.Contracts.IntegrationEvents.Hotels;

namespace Notification.WebApi.Consumers;

public class HousekeepingStatusChangedConsumer : IConsumer<HousekeepingStatusChangedIntegrationEvent>
{
    private readonly ITelegramNotifier _telegram;
    private readonly ILogger<HousekeepingStatusChangedConsumer> _logger;

    public HousekeepingStatusChangedConsumer(ITelegramNotifier telegram, ILogger<HousekeepingStatusChangedConsumer> logger)
    {
        _telegram = telegram;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<HousekeepingStatusChangedIntegrationEvent> context)
    {
        var evt = context.Message;

        _logger.LogInformation(
            "Housekeeping changed: Room {RoomNumber} ({PrevStatus} -> {NewStatus})",
            evt.RoomNumber, evt.PreviousStatus, evt.NewStatus);

        // Send telegram notification when room becomes Dirty
        if (evt.NewStatus == HousekeepingStatus.Dirty)
        {
            var hotelLabel = string.IsNullOrEmpty(evt.HotelName) ? "" : $"🏨 Отель: <b>{evt.HotelName}</b>\n";
            var message = "🧹 <b>Требуется уборка!</b>\n\n"
                        + hotelLabel
                        + $"🚪 Номер: <b>{evt.RoomNumber}</b>\n"
                        + $"📋 Статус: {evt.PreviousStatus} → <b>{evt.NewStatus}</b>\n"
                        + $"🕐 Время: {evt.Timestamp:dd.MM.yyyy HH:mm}\n\n"
                        + "Пожалуйста, выполните уборку номера.";

            await _telegram.SendMessageAsync(message);
        }
    }
}
