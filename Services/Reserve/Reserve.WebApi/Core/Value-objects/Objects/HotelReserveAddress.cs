using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReservService
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

        public string postIndex { get; private set; }
        public string country { get; private set; }
        public string city { get; private set; }
        public string street { get; private set; }
        public string buildNumber { get; private set; }

        public string GetFullAddress() => $"{postIndex};{country};{city};{street};{buildNumber}";


        public override bool Equals(object obj)
        {
            if (obj is HotelReserveAddress other)
            {
                return this.postIndex == other.postIndex &&
                       this.country == other.country &&
                       this.city == other.city &&
                       this.buildNumber == other.buildNumber &&
                       this.street == other.street;
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(postIndex,
                                                              country,
                                                              city,
                                                              street,
                                                              buildNumber);
    }
}
