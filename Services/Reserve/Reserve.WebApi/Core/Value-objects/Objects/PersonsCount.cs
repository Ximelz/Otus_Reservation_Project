using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
{
    public class PersonsCount
    {
        public PersonsCount(int adultCount, int childCount)
        {
            this.adultCount = adultCount;
            this.childCount = childCount;
        }
        public int adultCount { get; private set; }
        public int childCount { get; private set; }

        public int GetTotalCount() => adultCount + childCount;


        public override bool Equals(object obj)
        {
            if (obj is PersonsCount other)
            {
                return this.adultCount == other.adultCount &&
                       this.childCount == other.childCount;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(childCount, adultCount);
    }
}
