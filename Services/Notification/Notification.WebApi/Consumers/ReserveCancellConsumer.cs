using MassTransit;
using Notification.WebApi.Services;
using Shared.Contracts.IntegrationEvents.Hotels;
using Shared.Contracts.IntegrationEvents.Reservations;

namespace Notification.WebApi.Consumers
{
    public class ReserveCancellConsumer : IConsumer<ReservationCancelledIntegrationEvent>
    {
        private readonly ITelegramNotifier _telegram;
        private readonly ILogger<ReservationCancelledIntegrationEvent> _logger;

        public ReserveCancellConsumer(ITelegramNotifier telegram, ILogger<ReservationCancelledIntegrationEvent> logger)
        {
            _telegram = telegram;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationCancelledIntegrationEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation($"Reserve cancell: reason - {evt.Reason}, date - {evt.Timestamp:dd.MM.yyyy HH:mm}");

            var message = $"Бронь отменена! Причина {evt.Reason}, дата {evt.Timestamp:dd.MM.yyyy}";

            await _telegram.SendMessageAsync(message);
        }
    }
}
