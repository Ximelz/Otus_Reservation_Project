using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public static class RoomReserveMapper
    {
        public static RoomReserveModel MapToModel(this RoomReserve room) => new RoomReserveModel {
                                                                            Id = room.Id,
                                                                            RoomNumb = room.RoomNumb,
                                                                            Price = room.Price,
                                                                            Hotel = room.Hotel.MapToModel(),
                                                                            adultCount = room.Capacity.adultCount,
                                                                            childCount = room.Capacity.childCount,
                                                                            Status = room.Status };

        public static RoomReserve MapFromModel(this RoomReserveModel roomModel) => new RoomReserve(
                                                                                   roomModel.Id,
                                                                                   roomModel.Hotel.MapFromModel(),
                                                                                   roomModel.RoomNumb,
                                                                                   new RoomReserveCapacity(roomModel.adultCount, roomModel.childCount),
                                                                                   roomModel.Status,
                                                                                   roomModel.Price );

        public static List<RoomReserve> MapListFromModel(this List<RoomReserveModel> roomModelList)
        {
            var list = new List<RoomReserve>();
            foreach (var roomModel in roomModelList)
                list.Add(roomModel.MapFromModel());
            return list;
        }

        public static List<RoomReserveModel> MapListToModel(this List<RoomReserve> roomList)
        {
            var list = new List<RoomReserveModel>();
            foreach (var room in roomList)
                list.Add(room.MapToModel());
            return list;
        }
    }
}
