using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class RoomReserve
    {
        public RoomReserve(Guid Id, HotelReserve Hotel, string RoomNumb, RoomReserveCapacity Capacity, RoomReserveStatus Status, long price)
        {
            this.Id = Id;
            this.Hotel = Hotel;
            this.RoomNumb = RoomNumb;
            this.Capacity = Capacity;
            this.Status = Status;
            Price = price;
        }
        public readonly Guid Id;
        public readonly string RoomNumb;
        public readonly long Price;
        public HotelReserve Hotel { get; private set; }
        public RoomReserveCapacity Capacity { get; private set; }
        public RoomReserveStatus Status { get; private set; }
        public void ChangeRoomStatus(RoomReserveStatus newStatus) => Status = newStatus;
        public void ChangeCapacity(RoomReserveCapacity newCapacity) => Capacity = newCapacity;
    }
}
