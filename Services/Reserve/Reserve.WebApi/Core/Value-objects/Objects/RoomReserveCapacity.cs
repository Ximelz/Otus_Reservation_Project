using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class RoomReserveCapacity
    {
        public RoomReserveCapacity(int adultCount, int childCount)
        {
            this.adultCount = adultCount;
            this.childCount = childCount;
        }
        public int adultCount { get; private set; }
        public int childCount { get; private set; }

        public int GetCapacityRoom() => adultCount + childCount;

        public override bool Equals(object obj)
        {
            if (obj is RoomReserveCapacity other)
            {
                return this.adultCount == other.adultCount &&
                       this.childCount == other.childCount;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(childCount, adultCount);
    }
}
