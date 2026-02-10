using ReservService;
using System;
using System.Collections.Generic;
using System.Text;

namespace xUnitReservServiceTests
{
    public class TestObjectsInit
    {
        public static Reserve GetTestReserve(PersonReserve? person = null, RoomReserve? room = null)
        {
            Guid id = Guid.NewGuid();

            Random random = new Random();

            if (person == null)
                person = GetTestUser();

            if (room == null)
                room = GetTestRoom();

            DateTime checkIn = new DateTime(2026,
                                            random.Next(1, 6),
                                            random.Next(1, 28));

            DateTime checkOut = new DateTime(2026,
                                             random.Next(7, 12),
                                             random.Next(1, 30));

            Reserve reserve = new Reserve(id,
                                          person,
                                          room,
                                          checkIn,
                                          checkOut,
                                          StatusReserve.Active,
                                          new PersonsCount(2, 1),
                                          random.Next(25, 100));

            return reserve;
        }

        public static PersonReserve GetTestUser()
        {
            Guid id = Guid.NewGuid();

            PersonReserve person = new PersonReserve(id,
                                                     "testuser@temp.ru",
                                                     "8-888-888-88-88",
                                                     "TestName",
                                                     PersonReserveContactType.Telegram);
            return person;
        }


        public static RoomReserve GetTestRoom(HotelReserve? hotel = null)
        {
            Guid id = Guid.NewGuid();

            Random random = new Random();

            if (hotel == null)
                hotel = GetTestHotel();

            RoomReserve room = new RoomReserve(id,
                                               hotel,
                                               $"Room №{random.Next(1, 30)}",
                                               new RoomReserveCapacity(3, 3),
                                               RoomReserveStatus.Reserve,
                                               random.Next(25, 50));

            return room;
        }


        public static HotelReserve GetTestHotel()
        {
            Guid id = Guid.NewGuid();

            HotelReserve hotel = new HotelReserve(id,
                                                  "TestHotel",
                                                  "testhotel@temp.ru",
                                                  "8-888-777-77-77",
                                                  new HotelReserveAddress("HotelPostIndex",
                                                                          "HotelCountry",
                                                                          "HotelCity",
                                                                          "HotelStreet",
                                                                          "HotelBuildNumber"));

            return hotel;
        }
    }
}
