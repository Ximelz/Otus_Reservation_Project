using Core.Reserve;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Domain
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string dbConn = "Host=localhost;Database=ReserveService;Username=postgres;Password=12345;Port=5432";

            IDbContextFactory<ReservDbContext> dbContext = new DbContextFactory(dbConn);
            IReserveRepository repository = new PostgresSqlReserveRepository(dbContext);
            IReserveService service = new ReserveService(repository);

            CancellationTokenSource cts = new CancellationTokenSource();

            var reserve = ReserveMethodsToTest.CreateReserve();

            await service.RegisterReserve(reserve, cts.Token);

            var reservesFromDb = await service.GetReservesByUserId(reserve.UserReserve.Id, cts.Token);
            
            foreach(var reserveFromDb in reservesFromDb)
                Console.WriteLine(reserveFromDb.ReserveToString());
        }

        
    }

    public static class ReserveMethodsToTest
    {
        public static Reserve CreateReserve()
        {
            HotelReserve hotel = new HotelReserve(
                    Guid.NewGuid(),
                    "HotelName",
                    "HotelEmail",
                    "HotelPhone",
                    new HotelReserveAddress("HotelPostIndex", "HotelCountry", "HotelCity", "HotelStreet", "HotelBuildNumber"));

            RoomReserve room = new RoomReserve(
                    Guid.NewGuid(),
                    hotel,
                    "RoomNumb",
                    new RoomReserveCapacity(2, 1),
                    RoomReserveStatus.Reserve,
                    190);

            PersonReserve person = new PersonReserve(
                    Guid.NewGuid(),
                    "PersonEmail",
                    "PersonPhone",
                    "PersonName",
                    PersonReserveContactType.Telegram);

            Reserve reserve = new Reserve(
                    Guid.NewGuid(),
                    person,
                    room,
                    280,
                    new TimeSpan(5, 20, 00),
                    StatusReserve.Active,
                    new PersonsCount(2, 1));

            return reserve;
        }

        public static string ReserveToString(this Reserve reserve)
        {
            string resultStr = "";
            resultStr += $"ReserveInfo:\r\n" +
                         $"\tId - {reserve.Id}\r\n" +
                         $"\tCost - {reserve.Cost}\r\n" +
                         $"\tTime span - {reserve.ReserveTimeSpan}\r\n" +
                         $"\tStatus - {reserve.Status}\r\n" +
                         $"\tPersons - {reserve.Persons.GetTotalCount()}\r\n";

            resultStr += $"\r\nHotelInfo:\r\n" +
                         $"\tId - {reserve.RoomReserve.Hotel.Id}\r\n" +
                         $"\tName - {reserve.RoomReserve.Hotel.Name}\r\n" +
                         $"\tEmail - {reserve.RoomReserve.Hotel.Email}\r\n" +
                         $"\tPhone - {reserve.RoomReserve.Hotel.Phone}\r\n" +
                         $"\tAddress - {reserve.RoomReserve.Hotel.Address.GetFullAddress()}\r\n";

            resultStr += $"\r\nRoomInfo:\r\n" +
                         $"\tId - {reserve.RoomReserve.Id}\r\n" +
                         $"\tRoom number - {reserve.RoomReserve.RoomNumb}\r\n" +
                         $"\tPrice - {reserve.RoomReserve.Price}\r\n" +
                         $"\tCapacity - {reserve.RoomReserve.Capacity.GetCapacityRoom()}\r\n" +
                         $"\tStatus - {reserve.RoomReserve.Status}\r\n";

            resultStr += $"\r\nUserInfo:\r\n" +
                         $"\tId - {reserve.UserReserve.Id}\r\n" +
                         $"\tName - {reserve.UserReserve.Name}\r\n" +
                         $"\tEmail - {reserve.UserReserve.Email}\r\n" +
                         $"\tPhone - {reserve.UserReserve.Phone}\r\n" +
                         $"\tContact type - {reserve.UserReserve.ContactType}\r\n";

            resultStr += "---------------------------------------------------------";

            return resultStr;
        }
    }
}
