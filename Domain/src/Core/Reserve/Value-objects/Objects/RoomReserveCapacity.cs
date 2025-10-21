using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class RoomReserveCapacity
    {
        public RoomReserveCapacity(int adultCount, int childCount)
        {
            this.adultCount = adultCount;
            this.childCount = childCount;
        }
        public readonly int adultCount;
        public readonly int childCount;

        public int GetCapacityRoom() => adultCount + childCount;
    }
}
