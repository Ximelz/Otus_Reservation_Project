using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Admin.Ui.Services;

/// <summary>
/// Состояние авторизации для Admin API.
/// Использует HTTP-only cookies для сохранения токена между навигациями.
/// </summary>
public sealed class AdminApiState : INotifyPropertyChanged
{
    private const string TokenCookieName = "admin_api_token";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _accessToken;

    public AdminApiState(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        // Восстанавливаем токен из cookies при создании
        _accessToken = LoadTokenFromCookie();
    }

    public string? AccessToken
    {
        get
        {
            // Если токен уже загружен в память, используем его
            if (_accessToken != null)
                return _accessToken;
            
            // Иначе пытаемся загрузить из cookie
            var token = LoadTokenFromCookie();
            if (token != null)
            {
                _accessToken = token; // Кэшируем в памяти
            }
            return _accessToken;
        }
        private set
        {
            if (_accessToken != value)
            {
                _accessToken = value;
                SaveTokenToCookie(value);
                OnPropertyChanged(nameof(AccessToken));
                OnPropertyChanged(nameof(IsAuthenticated));
                AuthenticationStateChanged?.Invoke();
            }
        }
    }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action? AuthenticationStateChanged;

    public void SetToken(string token)
    {
        AccessToken = token;
    }

    public void Clear()
    {
        AccessToken = null;
    }

    private string? LoadTokenFromCookie()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request.Cookies.TryGetValue(TokenCookieName, out var token) == true)
        {
            return token;
        }
        return null;
    }

    private void SaveTokenToCookie(string? token)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var response = httpContext.Response;
        var request = httpContext.Request;
        
        if (string.IsNullOrWhiteSpace(token))
        {
            // Удаляем cookie
            response.Cookies.Delete(TokenCookieName);
        }
        else
        {
            // Сохраняем токен в HTTP-only cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Защита от XSS
                Secure = request.IsHttps, // HTTPS только если доступен
                SameSite = SameSiteMode.Lax, // Защита от CSRF
                Path = "/",
                // Expires не устанавливаем - cookie будет сессионной
            };

            response.Cookies.Append(TokenCookieName, token, cookieOptions);
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

