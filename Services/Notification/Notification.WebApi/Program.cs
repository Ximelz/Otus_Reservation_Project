using MassTransit;
using Notification.WebApi.Consumers;
using Notification.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Telegram Bot
builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("Telegram"));
builder.Services.AddSingleton<ITelegramNotifier, TelegramNotifier>();

// MassTransit with RabbitMQ
var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<HousekeepingStatusChangedConsumer>();
    x.AddConsumer<RoomStatusChangedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.MapControllers();
app.MapGet("/health", () => Results.Ok("OK"));
app.Run();
