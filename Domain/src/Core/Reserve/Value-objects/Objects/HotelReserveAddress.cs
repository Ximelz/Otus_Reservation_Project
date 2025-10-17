using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public class HotelReserveAddress
    {
        public HotelReserveAddress(string postIndex, string country, string city, string street, string buildNumber)
        {
            this.postIndex = postIndex;
            this.country = country;
            this.city = city;
            this.street = street;
            this.buildNumber = buildNumber;
        }

        public readonly string postIndex;
        public readonly string country;
        public readonly string city;
        public readonly string street;
        public readonly string buildNumber;

        public string GetFullAddress() => $"{postIndex};{country};{city};{street};{buildNumber}";
    }
}
