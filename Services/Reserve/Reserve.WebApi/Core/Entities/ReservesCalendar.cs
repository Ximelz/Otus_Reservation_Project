namespace ReservService
{
    public class ReservesCalendar
    {
        public ReservesCalendar(DateTime date1, DateTime date2, Guid roomId)
        {
            date1 = date1.Date;
            date2 = date2.Date;
            this.roomId = roomId;
            Dates = new();

            while (date1 <= date2)
            {
                Dates.Add(date1, false);
                date1.AddDays(1);
            }
        }
        public Guid roomId { get; init; }
        public Dictionary<DateTime, bool> Dates { get; private set; }
        public Task AddReservesInCalendar(IReadOnlyList<Reserve> reserves)
        {
            var currentReserves = reserves.Where(x => x.RoomReserve.Id == roomId).ToList();
            foreach (var reserve in currentReserves)
            {
                DateTime CheckIn = reserve.CheckIn.Date;
                DateTime CheckOut = reserve.CheckOut.Date;

                while (CheckIn <= CheckOut)
                {
                    if (Dates.ContainsKey(CheckIn))
                        Dates[CheckIn] = true;

                    CheckIn.AddDays(1);
                }
            }
            return Task.CompletedTask;
        }
    }
}
