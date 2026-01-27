using System.Net;
using System.Net.Http.Json;
using Admin.Application.Contracts.Hotels;
using Admin.Infrastructure.Clients;

namespace Admin.xUnitTests;

public sealed class HotelsClientTests
{
    [Fact]
    public async Task GetHotel_ReturnsHotel_On200()
    {
        var handler = new StubHttpMessageHandler(async (request, ct) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("/api/hotels/42", request.RequestUri?.PathAndQuery);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new HotelDto
                {
                    Id = 42,
                    Name = "Test Hotel",
                    Stars = 5,
                    CountryId = 1,
                    Address = "City, Street",
                    Phone = "+79990001122",
                    Email = "admin@test.local",
                    Description = "desc"
                })
            };
        });

        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://hotels.local") };
        var client = new HotelsClient(httpClient);

        var hotel = await client.GetHotel(42, CancellationToken.None);

        Assert.NotNull(hotel);
        Assert.Equal(42, hotel.Id);
        Assert.Equal("Test Hotel", hotel.Name);
    }

    [Fact]
    public async Task GetHotel_ReturnsNull_On404()
    {
        var handler = new StubHttpMessageHandler((request, ct) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });

        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://hotels.local") };
        var client = new HotelsClient(httpClient);

        var hotel = await client.GetHotel(999, CancellationToken.None);
        Assert.Null(hotel);
    }

    [Fact]
    public async Task UpsertHotel_SendsPost()
    {
        var handler = new StubHttpMessageHandler(async (request, ct) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("/api/hotels", request.RequestUri?.PathAndQuery);

            var body = await request.Content!.ReadAsStringAsync(ct);
            Assert.Contains("\"name\":\"X\"", body, StringComparison.OrdinalIgnoreCase);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://hotels.local") };
        var client = new HotelsClient(httpClient);

        await client.UpsertHotel(new HotelDto { Name = "X" }, CancellationToken.None);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

        public StubHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => _handler(request, cancellationToken);
    }
}

