namespace ReservService
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string RoomNumb { get; set; }
        public double Price { get; set; }
        public HotelDto Hotel { get; set; }
        public bool IsEnabled { get; set; }
        public int Capacity { get; set; }
    }
}
