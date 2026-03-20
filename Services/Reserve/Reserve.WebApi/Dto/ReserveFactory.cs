using System.Net.Http;

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
        private static RoomReserve GetRoom(Guid id)
        {
            HttpClient _httpClient = new HttpClient();
            var response = _httpClient.GetAsync($"http://localhost:12254/gateway/room/{id}").Result;

            if (!response.IsSuccessStatusCode)
                throw new ArgumentException();

            var Dto = response.Content.ReadFromJsonAsync<RoomDto>().Result;

            if (Dto == null || Dto.Hotel == null || !Dto.IsEnabled)
                throw new ArgumentException();

            RoomReserve room = new RoomReserve(Dto.Id, GetHotel(Dto.Hotel), Dto.RoomNumb, GetCapacity(Dto.Capacity), RoomReserveStatus.Reserve, Dto.Price);
            return room;
        }

        private static HotelReserve GetHotel(HotelDto dto) => new HotelReserve(dto.Id, dto.Name, dto.Email, dto.Phone, GetAddress(dto.Address));

        private static HotelReserveAddress GetAddress(string address)
        {
            string[] addressArray = address.Split(',');
            
            if (addressArray.Length < 1)
                return new HotelReserveAddress("", "", "", "", "");

            if (addressArray.Length < 2)
                return new HotelReserveAddress(addressArray[0], "", "", "", "");

            if (addressArray.Length < 3)
                return new HotelReserveAddress(addressArray[0], addressArray[1], "", "", "");

            if (addressArray.Length < 4)
                return new HotelReserveAddress(addressArray[0], addressArray[1], addressArray[2], "", "");

            if (addressArray.Length < 5)
                return new HotelReserveAddress(addressArray[0], addressArray[1], addressArray[2], addressArray[3], "");
            
            return new HotelReserveAddress(addressArray[0], addressArray[1], addressArray[2], addressArray[3], addressArray[4]);
        }

        private static RoomReserveCapacity GetCapacity() => new RoomReserveCapacity(2, 1);
        private static RoomReserveCapacity GetCapacity(int capacity) => new RoomReserveCapacity(capacity, 0);
    }
}