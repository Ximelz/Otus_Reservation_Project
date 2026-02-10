using ReservService;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace xUnitReservServiceTests
{
    public static class TestObjectsClone
    {
        public static Reserve GetClone(this Reserve reserve) => new Reserve(reserve.Id,
                                                                            reserve.UserReserve.GetClone(),
                                                                            reserve.RoomReserve.GetClone(),
                                                                            reserve.CheckIn,
                                                                            reserve.CheckOut,
                                                                            reserve.Status,
                                                                            reserve.Persons,
                                                                            reserve.Cost);

        public static RoomReserve GetClone(this RoomReserve room) => new RoomReserve(room.Id,
                                                                                     room.Hotel.GetClone(),
                                                                                     room.RoomNumb,
                                                                                     room.Capacity.GetClone(),
                                                                                     room.Status,
                                                                                     room.Price);

        public static HotelReserve GetClone(this HotelReserve hotel) => new HotelReserve(hotel.Id,
                                                                                         hotel.Name,
                                                                                         hotel.Email,
                                                                                         hotel.Phone,
                                                                                         hotel.Address.GetClone());

        public static PersonReserve GetClone(this PersonReserve person) => new PersonReserve(person.Id,
                                                                                             person.Email,
                                                                                             person.Phone,
                                                                                             person.Name,
                                                                                             person.ContactType);

        public static HotelReserveAddress GetClone(this HotelReserveAddress address) => new HotelReserveAddress(address.postIndex,
                                                                                                                address.country,
                                                                                                                address.city,
                                                                                                                address.street,
                                                                                                                address.buildNumber);

        public static PersonsCount GetClone(this PersonsCount count) => new PersonsCount(count.adultCount, count.childCount);
        public static RoomReserveCapacity GetClone(this RoomReserveCapacity capacity) => new RoomReserveCapacity(capacity.adultCount, capacity.childCount);
    }
}
