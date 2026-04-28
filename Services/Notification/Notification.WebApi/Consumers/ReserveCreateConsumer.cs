using MassTransit;
using Notification.WebApi.Services;
using Shared.Contracts.Enums;
using Shared.Contracts.IntegrationEvents.Hotels;
using Shared.Contracts.IntegrationEvents.Reservations;

namespace Notification.WebApi.Consumers
{
    public class ReserveCreateConsumer : IConsumer<ReservationCreatedIntegrationEvent>
    {
        private readonly ITelegramNotifier _telegram;
        private readonly ILogger<ReservationCreatedIntegrationEvent> _logger;

        public ReserveCreateConsumer(ITelegramNotifier telegram, ILogger<ReservationCreatedIntegrationEvent> logger)
        {
            _telegram = telegram;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationCreatedIntegrationEvent> context)
        {
            var evt = context.Message;

            _logger.LogInformation($"Reserve create: person - {evt.GuestName}, dates - [{evt.CheckInDate} : {evt.CheckOutDate}]");

            var message = $"Бронь создана! Гость {evt.GuestName}," +
                          $"дата заселения - {evt.CheckInDate}," +
                          $" дата выезда - {evt.CheckOutDate}," +
                          $" кличество гостей: взрослых - {evt.Adults}," +
                          $" количество детей = {evt.Children}.";
            
            await _telegram.SendMessageAsync(message);
        }
    }
}
