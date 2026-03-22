using Hotels.Domain.Repositories;
using Hotels.Domain.Services;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;
using Hotels.Infrastructure.Seed;
using Hotels.Infrastructure.Services;
using MassTransit;

namespace Hotels.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            PgDbContextOptions pgOptions = new();
            builder.Services.AddSingleton(pgOptions);

            IHotelsRepository hotelsRepository = new HotelsSqlRepository(pgOptions);
            builder.Services.AddSingleton(hotelsRepository);
            builder.Services.AddSingleton<IHotelsService, HotelsService>();

            IRoomsRepository roomsRepository = new RoomsSqlRepository(pgOptions);
            builder.Services.AddSingleton(roomsRepository);
            builder.Services.AddSingleton<IRoomsService, RoomsService>();

            IRoomTypesRepository roomTypesRepository = new RoomTypesSqlRepository(pgOptions);
            builder.Services.AddSingleton(roomTypesRepository);
            builder.Services.AddSingleton<IRoomTypesService, RoomTypesService>();

            IRatePlansRepository ratesRepository = new RatePlansSqlRepository(pgOptions);
            builder.Services.AddSingleton(ratesRepository);
            builder.Services.AddSingleton<IRatePlansService, RatePlansService>();

            ISeasonPriceRepository seasonPriceRepository = new SeasonPriceSqlRepository(pgOptions);
            builder.Services.AddSingleton(seasonPriceRepository);
            builder.Services.AddSingleton<ISeasonPriceService, SeasonPriceService>();

            // MassTransit with RabbitMQ
            var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

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

            // Seed data
            builder.Services.AddHostedService<HotelsSeedService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c => c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();
            app.MapGet("/health", () => Results.Ok("OK"));
            app.Run();
        }
    }
}
