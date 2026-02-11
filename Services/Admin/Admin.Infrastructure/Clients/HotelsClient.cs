using System.Net;
using System.Net.Http.Json;
using Admin.Application.Abstractions;
using Admin.Application.Contracts.Hotels;

namespace Admin.Infrastructure.Clients;

public sealed class HotelsClient : IHotelsClient
{
    private readonly HttpClient _httpClient;

    public HotelsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HotelDto?> GetHotel(long id, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"/api/hotels/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HotelDto>(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<HotelDto>> GetHotelsByCountry(int countryId, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"/api/hotels/byCountry/{countryId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<HotelDto>>(cancellationToken: cancellationToken)) ?? [];
    }

    public async Task<IReadOnlyList<HotelDto>> GetHotelsByStars(int stars, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"/api/hotels/byStars/{stars}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<HotelDto>>(cancellationToken: cancellationToken)) ?? [];
    }

    public async Task UpsertHotel(HotelDto hotel, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync("/api/hotels", hotel, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteHotel(long id, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.DeleteAsync($"/api/hotels/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<RoomDto?> GetRoom(long id, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"/api/rooms/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RoomDto>(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<RoomDto>> GetRoomsByHotel(long hotelId, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"/api/rooms/hotel/{hotelId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<RoomDto>>(cancellationToken: cancellationToken)) ?? [];
    }

    public async Task UpsertRoom(RoomDto room, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.PostAsJsonAsync("/api/rooms", room, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRoom(long id, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.DeleteAsync($"/api/rooms/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRoomsByHotel(long hotelId, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.DeleteAsync($"/api/rooms/hotel/{hotelId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}

