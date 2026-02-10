namespace ReservService
{
    public class ReserveDto
    {
        public Guid userId { get; set; }
        public Guid roomId { get; set; }
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public int adultCount { get; set; }
        public int childCount { get; set; }
    }
}