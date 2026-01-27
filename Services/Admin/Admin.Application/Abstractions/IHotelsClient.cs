using Admin.Application.Contracts.Hotels;

namespace Admin.Application.Abstractions;

public interface IHotelsClient
{
    Task<HotelDto?> GetHotel(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<HotelDto>> GetHotelsByCountry(int countryId, CancellationToken cancellationToken);
    Task<IReadOnlyList<HotelDto>> GetHotelsByStars(int stars, CancellationToken cancellationToken);
    Task UpsertHotel(HotelDto hotel, CancellationToken cancellationToken);
    Task DeleteHotel(long id, CancellationToken cancellationToken);

    Task<RoomDto?> GetRoom(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<RoomDto>> GetRoomsByHotel(long hotelId, CancellationToken cancellationToken);
    Task UpsertRoom(RoomDto room, CancellationToken cancellationToken);
    Task DeleteRoom(long id, CancellationToken cancellationToken);
    Task DeleteRoomsByHotel(long hotelId, CancellationToken cancellationToken);
}

