namespace ReservService
{
    public interface IReservesCalendarService
    {
        public Task<IReadOnlyList<ReservesCalendar>> GetReserveCalendarByCity(DateTime date1, DateTime date2, string city, CancellationToken ct);
        public Task<IReadOnlyList<ReservesCalendar>> GetReserveCalendarByHotel(DateTime date1, DateTime date2, Guid id, CancellationToken ct);
        public Task<IReadOnlyList<ReservesCalendar>> GetReserveCalendarByCountry(DateTime date1, DateTime date2, string country, CancellationToken ct);
    }
}
