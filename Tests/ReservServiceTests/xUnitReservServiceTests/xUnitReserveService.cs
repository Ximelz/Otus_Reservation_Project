using ReservService;

namespace xUnitReservServiceTests
{
    public class xUnitReserveService
    {
        private readonly IReserveService _service;
        public xUnitReserveService()
        {
            string dbConn = "Host=localhost;Database=ReserveServiceTest;Username=postgres;Password=12345;Port=5432";
            IDbContextFactory<ReservDbContext> dbContext = new DbContextFactory(dbConn);
            IReserveRepository _repository = new PostgresSqlReserveRepository(dbContext);
            IReserveRepository _repositoryInMemory = new InMemoryReserveRepository();
            _service = new ReserveService(_repository);

            using (var context = dbContext.CreateDbContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task TestAdd()
        {
            Reserve? reserve = TestObjectsInit.GetTestReserve();
            Reserve tempReserve = reserve.GetClone();
            CancellationToken ct = CancellationToken.None;

            try
            {
                await _service.RegisterReserve(reserve, ct);
                reserve = null;
                reserve = await _service.GetReserveById(tempReserve.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            Assert.NotNull(reserve);
            Assert.Equal(reserve, tempReserve);
        }

        [Fact]
        public async Task TestUpdate()
        {
            Reserve? reserve = TestObjectsInit.GetTestReserve();
            Reserve tempReserve = reserve.GetClone();
            CancellationToken ct = CancellationToken.None;

            try
            {
                await _service.RegisterReserve(reserve, ct);
                reserve = null;
                reserve = await _service.GetReserveById(tempReserve.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            Assert.NotNull(reserve);

            reserve.ChangeDates(tempReserve.CheckIn.AddDays(5), tempReserve.CheckOut.AddDays(5));

            try
            {
                await _service.UpdateReserve(reserve, ct);
                reserve = null;
                reserve = await _service.GetReserveById(tempReserve.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            Assert.NotNull(reserve);

            if (reserve.Id != tempReserve.Id)
                Assert.Fail("ID не совпадают после обновления!");

            if (reserve.Status != tempReserve.Status)
                Assert.Fail("Status не совпадают после обновления!");

            if (reserve.Cost != tempReserve.Cost)
                Assert.Fail("Cost не совпадают после обновления!");

            if (!reserve.Persons.Equals(tempReserve.Persons))
                Assert.Fail("Persons не совпадают после обновления!");

            if (!reserve.UserReserve.Equals(tempReserve.UserReserve))
                Assert.Fail("UserReserve не совпадают после обновления!");

            if (!reserve.RoomReserve.Equals(tempReserve.RoomReserve))
                Assert.Fail("RoomReserve не совпадают после обновления!");

            Assert.NotEqual(tempReserve, reserve);
        }

        [Fact]
        public async Task TestCancel()
        {
            Reserve? reserve = TestObjectsInit.GetTestReserve();
            Reserve tempReserve = reserve.GetClone();
            CancellationToken ct = CancellationToken.None;

            reserve.ChangeReserveStatus(StatusReserve.AwaitPay);

            try
            {
                await _service.RegisterReserve(reserve, ct);
                await _service.CancelReserve(reserve.Id, ct);

                reserve = null;
                reserve = await _service.GetReserveById(tempReserve.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            Assert.NotNull(reserve);

            if (reserve.Status != StatusReserve.Cancel)
                Assert.Fail("Status не поменялся на Cancel.");

            tempReserve.ChangeReserveStatus(reserve.Status);

            Assert.Equal(reserve, reserve);
        }

        [Fact]
        public async Task TestGetByUser()
        {
            PersonReserve firstTestPerson = TestObjectsInit.GetTestUser();
            PersonReserve secondTestPerson = TestObjectsInit.GetTestUser();

            List<Reserve> reservesAddFirstUser = new List<Reserve>();
            List<Reserve> reservesAddSecondUser = new List<Reserve>();
            List<Reserve> reservesGet = new List<Reserve>();

            CancellationToken ct = CancellationToken.None;

            for (int i = 0; i < 10; i++)
                reservesAddFirstUser.Add(TestObjectsInit.GetTestReserve(firstTestPerson));

            for (int i = 0; i < 10; i++)
                reservesAddSecondUser.Add(TestObjectsInit.GetTestReserve(secondTestPerson));

            try
            {
                foreach (Reserve reserve in reservesAddFirstUser)
                    await _service.RegisterReserve(reserve.GetClone(), ct);

                foreach (Reserve reserve in reservesAddSecondUser)
                    await _service.RegisterReserve(reserve.GetClone(), ct);

                reservesGet = await _service.GetReservesByUserId(firstTestPerson.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            foreach (Reserve reserve in reservesGet)
                if (!reservesAddFirstUser.Contains(reserve))
                    Assert.Fail($"Не все брони тестового пользователя были добавлены в базу!");

            try
            {
                reservesGet = await _service.GetReservesByUserId(secondTestPerson.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }


            foreach (Reserve reserve in reservesGet)
                if (!reservesAddSecondUser.Contains(reserve))
                    Assert.Fail($"Не все брони тестового пользователя были добавлены в базу!");
        }

        [Fact]
        public async Task TestGetByHotel()
        {
            HotelReserve firstTestHotel = TestObjectsInit.GetTestHotel();
            HotelReserve secondTestHotel = TestObjectsInit.GetTestHotel();

            RoomReserve firstTestRoom = TestObjectsInit.GetTestRoom(firstTestHotel);
            RoomReserve secondTestRoom = TestObjectsInit.GetTestRoom(secondTestHotel);

            List<Reserve> reservesAddFirstHotel = new List<Reserve>();
            List<Reserve> reservesAddSecondHotel = new List<Reserve>();
            List<Reserve> reservesGet = new List<Reserve>();

            CancellationToken ct = CancellationToken.None;

            for (int i = 0; i < 10; i++)
                reservesAddFirstHotel.Add(TestObjectsInit.GetTestReserve(null,  firstTestRoom));

            for (int i = 0; i < 10; i++)
                reservesAddSecondHotel.Add(TestObjectsInit.GetTestReserve(null, secondTestRoom));

            try
            {
                foreach (Reserve reserve in reservesAddFirstHotel)
                    await _service.RegisterReserve(reserve.GetClone(), ct);

                foreach (Reserve reserve in reservesAddSecondHotel)
                    await _service.RegisterReserve(reserve.GetClone(), ct);

                reservesGet = await _service.GetReservesByHotelId(firstTestHotel.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            foreach (Reserve reserve in reservesGet)
                if (!reservesAddFirstHotel.Contains(reserve))
                    Assert.Fail($"Не все брони тестового пользователя были добавлены в базу!");

            try
            {
                reservesGet = await _service.GetReservesByHotelId(secondTestHotel.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }


            foreach (Reserve reserve in reservesGet)
                if (!reservesAddSecondHotel.Contains(reserve))
                    Assert.Fail($"Не все брони тестового пользователя были добавлены в базу!");
        }

        [Fact]
        public async Task TestGetByRoom()
        {
            RoomReserve firstTestRoom = TestObjectsInit.GetTestRoom();
            RoomReserve secondTestRoom = TestObjectsInit.GetTestRoom();

            List<Reserve> reservesAddFirstRoom = new List<Reserve>();
            List<Reserve> reservesAddSecondRoom = new List<Reserve>();
            List<Reserve> reservesGet = new List<Reserve>();

            CancellationToken ct = CancellationToken.None;

            for (int i = 0; i < 10; i++)
                reservesAddFirstRoom.Add(TestObjectsInit.GetTestReserve(null, firstTestRoom));

            for (int i = 0; i < 10; i++)
                reservesAddSecondRoom.Add(TestObjectsInit.GetTestReserve(null, secondTestRoom));

            try
            {
                foreach (Reserve reserve in reservesAddFirstRoom)
                    await _service.RegisterReserve(reserve.GetClone(), ct);

                foreach (Reserve reserve in reservesAddSecondRoom)
                    await _service.RegisterReserve(reserve.GetClone(), ct);

                reservesGet = await _service.GetReservesByRoomId(firstTestRoom.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            foreach (Reserve reserve in reservesGet)
                if (!reservesAddFirstRoom.Contains(reserve))
                    Assert.Fail($"Не все брони тестового пользователя были добавлены в базу!");

            try
            {
                reservesGet = await _service.GetReservesByRoomId(secondTestRoom.Id, ct);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }


            foreach (Reserve reserve in reservesGet)
                if (!reservesAddSecondRoom.Contains(reserve))
                    Assert.Fail($"Не все брони тестового пользователя были добавлены в базу!");
        }
    }
}
