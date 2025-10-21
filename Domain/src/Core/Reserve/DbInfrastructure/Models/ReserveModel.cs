using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class ReserveModel
    {
        public Guid Id { get; set; }
        public PersonReserveModel UserReserve { get; set; }
        public RoomReserveModel RoomReserve { get; set; }
        public long Cost { get; set; }
        public TimeSpan ReserveTimeSpan { get; set; }
        public StatusReserve Status { get; set; }
        public int adultCount { get; set; }
        public int childCount { get; set; }
    }
}
