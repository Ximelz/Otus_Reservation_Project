using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class ReserveModel
    {
        public Guid Id { get; set; }
        public PersonReserveModel UserReserve { get; set; }
        public RoomReserveModel RoomReserve { get; set; }
        public double Cost { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public StatusReserve Status { get; set; }
        public int adultCount { get; set; }
        public int childCount { get; set; }
    }
}
