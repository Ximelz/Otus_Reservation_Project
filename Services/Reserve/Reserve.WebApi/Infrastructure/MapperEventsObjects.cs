using Shared.Contracts.IntegrationEvents.Reservations;

namespace ReservService
{
    public static class MapperEventsObjects
    {
        public static ReservationCreatedIntegrationEvent CreateEvent(this Reserve reserve) => new ReservationCreatedIntegrationEvent()
        {
            ReservationId = reserve.Id,
            HotelId = reserve.RoomReserve.Hotel.Id,
            RoomId = reserve.RoomReserve.Id,
            GuestName = reserve.UserReserve.Name,
            CheckInDate = DateOnly.FromDateTime(reserve.CheckIn),
            CheckOutDate = DateOnly.FromDateTime(reserve.CheckOut),
            Adults = reserve.Persons.adultCount,
            Children = reserve.Persons.childCount
        };

        public static ReservationCancelledIntegrationEvent CancellEvent(this Reserve reserve, string reason) => new ReservationCancelledIntegrationEvent()
        {
            ReservationId = reserve.Id,
            HotelId = reserve.RoomReserve.Hotel.Id,
            RoomId = reserve.RoomReserve.Id,
            Reason = reason
        };
    }
}