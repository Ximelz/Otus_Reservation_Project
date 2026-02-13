using Hotels.Domain.Repositories;
using Hotels.Domain.Services;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;
using Hotels.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Hotels.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            PgDbContextOptions pgOptions = new();

            using (PgDbContext pgContext = new PgDbContext(pgOptions))
            {
                pgContext.Database.Migrate();
            }

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

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(
                    c => c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
