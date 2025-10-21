using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reserve
{
    public static class HotelReserveMapper
    {
        public static HotelReserveModel MapToModel(this HotelReserve hotel) => new HotelReserveModel {
                                                                               Id = hotel.Id,
                                                                               Name = hotel.Name,
                                                                               Email = hotel.Email,
                                                                               Phone = hotel.Phone,
                                                                               postIndex = hotel.Address.postIndex,
                                                                               country = hotel.Address.country,
                                                                               city = hotel.Address.city,
                                                                               street = hotel.Address.street,
                                                                               buildNumber = hotel.Address.buildNumber };

        public static HotelReserve MapFromModel(this HotelReserveModel hotelModel) => new HotelReserve(
                                                                                      hotelModel.Id,
                                                                                      hotelModel.Name,
                                                                                      hotelModel.Email,
                                                                                      hotelModel.Phone,
                                                                                      new HotelReserveAddress(
                                                                                            hotelModel.postIndex,
                                                                                            hotelModel.country,
                                                                                            hotelModel.city,
                                                                                            hotelModel.street,
                                                                                            hotelModel.buildNumber));

        public static List<HotelReserve> MapListFromModel(this List<HotelReserveModel> hotelModelList)
        {
            var list = new List<HotelReserve>();
            foreach (var hotelModel in hotelModelList)
                list.Add(hotelModel.MapFromModel());
            return list;
        }

        public static List<HotelReserveModel> MapListToModel(this List<HotelReserve> hotelList)
        {
            var list = new List<HotelReserveModel>();
            foreach (var hotel in hotelList)
                list.Add(hotel.MapToModel());
            return list;
        }
    }
}
