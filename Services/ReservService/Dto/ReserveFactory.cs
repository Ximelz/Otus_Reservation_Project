namespace ReservService
{
    public class ReserveFactory
    {
        public static Reserve CreateReserve(ReserveDto dto)
        {
            var person = GetPerson(dto.userId);
            var room = GetRoom(dto.roomId);
            return new Reserve(
                       Guid.NewGuid(),
                       person,
                       room,
                       dto.checkIn,
                       dto.checkOut,
                       StatusReserve.AwaitPay,
                       new PersonsCount(dto.adultCount, dto.childCount),
                       0);
        }
        private static PersonReserve GetPerson(Guid id) => new PersonReserve(id, "temp@email.ru", "8-888-888-88-88", "Name", PersonReserveContactType.Phone);
        private static RoomReserve GetRoom(Guid id) => new RoomReserve(
                                                           id,
                                                           new HotelReserve(
                                                               Guid.NewGuid(),
                                                               "Hotel",
                                                               "temp@email.ru",
                                                               "8-888-888-88-88",
                                                               new HotelReserveAddress(
                                                                   "Index",
                                                                   "Country",
                                                                   "City",
                                                                   "Street",
                                                                   "Number")),
                                                           "RoomNumber",
                                                           new RoomReserveCapacity(2, 1),
                                                           RoomReserveStatus.Free,
                                                           150);
    }
}