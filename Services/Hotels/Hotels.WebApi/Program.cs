using Hotels.Domain.Repositories;
using Hotels.Domain.Services;
using Hotels.Infrastructure;
using Hotels.Infrastructure.Repositories;
using Hotels.Infrastructure.Services;

namespace Hotels.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            PgDbContextOptions pgOptions = new();
            IHotelsRepository hotelsRepository = new HotelsSqlRepository(pgOptions);

            builder.Services.AddSingleton(hotelsRepository);
            builder.Services.AddSingleton<IHotelsService, HotelsService>();

            IRoomsRepository roomsRepository = new RoomsSqlRepository(pgOptions);

            builder.Services.AddSingleton(roomsRepository);
            builder.Services.AddSingleton<IRoomsService, RoomsService>();
            
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
