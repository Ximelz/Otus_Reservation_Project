namespace ReservService
{
    public class RoomReserve
    {
        public RoomReserve(Guid Id, HotelReserve Hotel, string RoomNumb, RoomReserveCapacity Capacity, RoomReserveStatus Status, double price)
        {
            this.Id = Id;
            this.Hotel = Hotel;
            this.RoomNumb = RoomNumb;
            this.Capacity = Capacity;
            this.Status = Status;
            Price = price;
        }
        public Guid Id { get; private set; }
        public string RoomNumb { get; private set; }
        public double Price { get; private set; }
        public HotelReserve Hotel { get; private set; }
        public RoomReserveCapacity Capacity { get; private set; }
        public RoomReserveStatus Status { get; private set; }
        public void ChangeRoomStatus(RoomReserveStatus newStatus) => Status = newStatus;
        public void ChangeCapacity(RoomReserveCapacity newCapacity) => Capacity = newCapacity;

        public override bool Equals(object obj)
        {
            if (obj is RoomReserve other)
            {
                return this.Hotel.Equals(other.Hotel) &&
                       this.Capacity.Equals(other.Capacity) &&
                       this.RoomNumb == other.RoomNumb &&
                       this.Status == other.Status &&
                       this.Price == other.Price &&
                       this.Id == other.Id;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Hotel.GetHashCode(),
                                                              Capacity.GetHashCode(),
                                                              Id,
                                                              Status,
                                                              RoomNumb,
                                                              Price);
    }
}
