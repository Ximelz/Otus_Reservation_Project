namespace ReservService
{
    public class ReservesCalendarService : IReservesCalendarService
    {
        private readonly IReserveRepository _reserveRepository;
        private readonly IRoomRepository _roomRepository;
        public ReservesCalendarService(IReserveRepository reserveRepository, IRoomRepository roomRepository)
        {
            _reserveRepository = reserveRepository;
            _roomRepository = roomRepository;
        }
        public async Task<IReadOnlyList<ReservesCalendar>> GetReserveCalendarByCity(DateTime date1, DateTime date2, string city, CancellationToken ct)
        {
            List<ReservesCalendar> calendars = new List<ReservesCalendar>();
            Func<Reserve, bool> predicate = x => (x.CheckOut.Date > date1 || x.CheckIn.Date < date2) && (x.RoomReserve.Hotel.Address.city == city);

            var reserves = await _reserveRepository.GetListAsync(predicate, ct);
            var rooms = await _roomRepository.GetRooms(x => x.Hotel.Address.city == city, ct);

            foreach (var room in rooms)
            {
                ReservesCalendar calendar = new ReservesCalendar(date1, date2, room);
                await calendar.AddReservesInCalendar(reserves);
                calendars.Add(calendar);
            }

            return calendars;
        }

        public async Task<IReadOnlyList<ReservesCalendar>> GetReserveCalendarByHotel(DateTime date1, DateTime date2, Guid id, CancellationToken ct)
        {
            List<ReservesCalendar> calendars = new List<ReservesCalendar>();
            Func<Reserve, bool> predicate = x => (x.CheckOut.Date > date1 || x.CheckIn.Date < date2) && (x.RoomReserve.Hotel.Id == id);

            var reserves = await _reserveRepository.GetListAsync(predicate, ct);
            var rooms = await _roomRepository.GetRooms(x => x.Hotel.Id == id, ct);

            foreach (var room in rooms)
            {
                ReservesCalendar calendar = new ReservesCalendar(date1, date2, room);
                await calendar.AddReservesInCalendar(reserves);
                calendars.Add(calendar);
            }

            return calendars;
        }

        public async Task<IReadOnlyList<ReservesCalendar>> GetReserveCalendarByCountry(DateTime date1, DateTime date2, string country, CancellationToken ct)
        {
            List<ReservesCalendar> calendars = new List<ReservesCalendar>();
            Func<Reserve, bool> predicate = x => (x.CheckOut.Date > date1 || x.CheckIn.Date < date2) && (x.RoomReserve.Hotel.Address.country == country);

            var reserves = await _reserveRepository.GetListAsync(predicate, ct);
            var rooms = await _roomRepository.GetRooms(x => x.Hotel.Address.country == country, ct);

            foreach (var room in rooms)
            {
                ReservesCalendar calendar = new ReservesCalendar(date1, date2, room);
                await calendar.AddReservesInCalendar(reserves);
                calendars.Add(calendar);
            }

            return calendars;
        }
    }
}
