using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Admin.Ui.Models;

namespace Admin.Ui.Services;

public sealed class AdminApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AdminApiState _state;

    public AdminApiClient(HttpClient httpClient, AdminApiState state)
    {
        _httpClient = httpClient;
        _state = state;
    }

    public async Task<AdminApiResult<AdminLoginResponse>> Login(string username, string password, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/admin/auth/login",
                new AdminLoginRequest { Username = username, Password = password }, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return AdminApiResult<AdminLoginResponse>.Fail("DevAuth отключен на API. Убедитесь, что запущено в Development окружении и DevAuth:Enabled=true.");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return AdminApiResult<AdminLoginResponse>.Fail($"Неверный логин или пароль. Проверьте credentials в appsettings.Development.json");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return AdminApiResult<AdminLoginResponse>.Fail($"Ошибка входа: {(int)response.StatusCode} {response.ReasonPhrase}. {errorContent}");
            }

            var payload = await response.Content.ReadFromJsonAsync<AdminLoginResponse>(cancellationToken: cancellationToken);
            if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
            {
                return AdminApiResult<AdminLoginResponse>.Fail("Неверный ответ от сервера: отсутствует токен.");
            }

            _state.SetToken(payload.AccessToken);
            return AdminApiResult<AdminLoginResponse>.Ok(payload);
        }
        catch (HttpRequestException ex)
        {
            return AdminApiResult<AdminLoginResponse>.Fail($"Ошибка подключения к API: {ex.Message}. Убедитесь, что Admin.WebApi запущен на http://localhost:5009");
        }
        catch (TaskCanceledException)
        {
            return AdminApiResult<AdminLoginResponse>.Fail($"Таймаут подключения к API. Убедитесь, что Admin.WebApi запущен и доступен.");
        }
        catch (Exception ex)
        {
            return AdminApiResult<AdminLoginResponse>.Fail($"Неожиданная ошибка: {ex.Message}");
        }
    }

    public void Logout() => _state.Clear();

    public async Task<AdminApiResult<IReadOnlyList<SystemSetting>>> GetSettings(CancellationToken cancellationToken)
        => await GetAsync<IReadOnlyList<SystemSetting>>("/api/admin/settings", cancellationToken);

    public async Task<AdminApiResult<SystemSetting>> UpsertSetting(string key, string value, CancellationToken cancellationToken)
        => await PutAsync<SystemSetting>($"/api/admin/settings/{Uri.EscapeDataString(key)}",
            new UpsertSystemSettingRequest { Value = value }, cancellationToken);

    public async Task<AdminApiResult<bool>> DeleteSetting(string key, CancellationToken cancellationToken)
    {
        var result = await DeleteAsync($"/api/admin/settings/{Uri.EscapeDataString(key)}", cancellationToken);
        return result.Success ? AdminApiResult<bool>.Ok(true) : AdminApiResult<bool>.Fail(result.Error ?? "Delete failed.");
    }

    public async Task<AdminApiResult<IReadOnlyList<HotelDto>>> GetHotelsByCountry(int countryId, CancellationToken cancellationToken)
        => await GetAsync<IReadOnlyList<HotelDto>>($"/api/admin/hotels/byCountry/{countryId}", cancellationToken);

    public async Task<AdminApiResult<IReadOnlyList<HotelDto>>> GetHotelsByStars(int stars, CancellationToken cancellationToken)
        => await GetAsync<IReadOnlyList<HotelDto>>($"/api/admin/hotels/byStars/{stars}", cancellationToken);

    public async Task<AdminApiResult<object>> UpsertHotel(HotelDto hotel, CancellationToken cancellationToken)
        => await PostAsync("/api/admin/hotels", hotel, cancellationToken);

    public async Task<AdminApiResult<object>> DeleteHotel(long id, CancellationToken cancellationToken)
        => await DeleteAsync($"/api/admin/hotels/{id}", cancellationToken);

    public async Task<AdminApiResult<IReadOnlyList<RoomDto>>> GetRoomsByHotel(long hotelId, CancellationToken cancellationToken)
        => await GetAsync<IReadOnlyList<RoomDto>>($"/api/admin/rooms/hotel/{hotelId}", cancellationToken);

    public async Task<AdminApiResult<object>> UpsertRoom(RoomDto room, CancellationToken cancellationToken)
        => await PostAsync("/api/admin/rooms", room, cancellationToken);

    public async Task<AdminApiResult<object>> DeleteRoom(long id, CancellationToken cancellationToken)
        => await DeleteAsync($"/api/admin/rooms/{id}", cancellationToken);

    public async Task<AdminApiResult<AdminDashboardResponse>> GetDashboard(int? countryId, int? stars, CancellationToken cancellationToken)
    {
        var query = new List<string>();
        if (countryId.HasValue)
        {
            query.Add($"countryId={countryId.Value}");
        }

        if (stars.HasValue)
        {
            query.Add($"stars={stars.Value}");
        }

        var path = "/api/admin/dashboard";
        if (query.Count > 0)
        {
            path += "?" + string.Join("&", query);
        }

        return await GetAsync<AdminDashboardResponse>(path, cancellationToken);
    }

    private void ApplyAuthHeader()
    {
        if (string.IsNullOrWhiteSpace(_state.AccessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _state.AccessToken);
    }

    private async Task<AdminApiResult<T>> GetAsync<T>(string path, CancellationToken cancellationToken)
    {
        ApplyAuthHeader();
        using var response = await _httpClient.GetAsync(path, cancellationToken);
        return await ReadResult<T>(response, cancellationToken);
    }

    private async Task<AdminApiResult<T>> PutAsync<T>(string path, object body, CancellationToken cancellationToken)
    {
        ApplyAuthHeader();
        using var response = await _httpClient.PutAsJsonAsync(path, body, cancellationToken);
        return await ReadResult<T>(response, cancellationToken);
    }

    private async Task<AdminApiResult<object>> PostAsync(string path, object body, CancellationToken cancellationToken)
    {
        ApplyAuthHeader();
        using var response = await _httpClient.PostAsJsonAsync(path, body, cancellationToken);
        return response.IsSuccessStatusCode
            ? AdminApiResult<object>.Ok(new object())
            : AdminApiResult<object>.Fail($"Request failed: {(int)response.StatusCode}");
    }

    private async Task<AdminApiResult<object>> DeleteAsync(string path, CancellationToken cancellationToken)
    {
        ApplyAuthHeader();
        using var response = await _httpClient.DeleteAsync(path, cancellationToken);
        return response.IsSuccessStatusCode
            ? AdminApiResult<object>.Ok(new object())
            : AdminApiResult<object>.Fail($"Request failed: {(int)response.StatusCode}");
    }

    private static async Task<AdminApiResult<T>> ReadResult<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var errorMessage = response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                ? "Ошибка авторизации (401). Токен недействителен или истек. Выполните вход снова."
                : $"Request failed: {(int)response.StatusCode} {response.ReasonPhrase}";
            
            if (!string.IsNullOrWhiteSpace(errorContent) && errorContent.Length < 200)
            {
                errorMessage += $" {errorContent}";
            }
            
            return AdminApiResult<T>.Fail(errorMessage);
        }

        var payload = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        if (payload is null)
        {
            return AdminApiResult<T>.Fail("Empty response.");
        }

        return AdminApiResult<T>.Ok(payload);
    }
}

