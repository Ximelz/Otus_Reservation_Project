namespace Core.Reserve
{
    public static class ReserveMapper
    {
        public static ReserveModel MapToModel(this Reserve reserve) => new ReserveModel {
                                                                       Id = reserve.Id,
                                                                       UserReserve = reserve.UserReserve.MapToModel(),
                                                                       RoomReserve = reserve.RoomReserve.MapToModel(),
                                                                       Cost = reserve.Cost,
                                                                       ReserveTimeSpan = reserve.ReserveTimeSpan,
                                                                       Status = reserve.Status,
                                                                       adultCount = reserve.Persons.adultCount,
                                                                       childCount = reserve.Persons.childCount };

        public static Reserve MapFromModel(this ReserveModel reserveModel) => new Reserve (
                                                                              reserveModel.Id,
                                                                              reserveModel.UserReserve.MapFromModel(),
                                                                              reserveModel.RoomReserve.MapFromModel(),
                                                                              reserveModel.Cost,
                                                                              reserveModel.ReserveTimeSpan,
                                                                              reserveModel.Status,
                                                                              new PersonsCount(reserveModel.adultCount, reserveModel.childCount) );

        public static List<Reserve> MapListFromModel(this List<ReserveModel> reserveModelList)
        {
            var list = new List<Reserve>();
            foreach (var reserveModel in reserveModelList)
                list.Add(reserveModel.MapFromModel());
            return list;
        }

        public static List<ReserveModel> MapListToModel(this List<Reserve> reserveList)
        {
            var list = new List<ReserveModel>();
            foreach (var reserve in reserveList)
                list.Add(reserve.MapToModel());
            return list;
        }
    }
}
