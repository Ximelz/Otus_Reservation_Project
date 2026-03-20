using Hotels.Domain.Entities;
using Hotels.Infrastructure.Services;

namespace Hotels.WebApi.Dto
{
    public class RoomDtoFactory
    {
        public static RoomDto GetRoomDto(Room room, Hotel hotel, double price, int capacity) => new RoomDto() { Id = room.Id,
                                                                                                                Hotel = GetHotelDto(hotel),
                                                                                                                RoomNumb = room.Number,
                                                                                                                Price = price,
                                                                                                                IsEnabled = room.IsEnabled,
                                                                                                                Capacity = capacity};

        private static HotelDto GetHotelDto(Hotel hotel) => new HotelDto() {  Id = hotel.Id,
                                                                              Name = hotel.Name,
                                                                              Email = hotel.Email,
                                                                              Phone = hotel.Phone,
                                                                              Address = hotel.Address };
    }
}
