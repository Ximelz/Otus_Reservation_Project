using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace ReservService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder();
                var dbConnStrategy = new DBConnStrStrategy("docker-compose");
                //var dbConnStrategy = new DBConnStrStrategy("local");
                IConnectString dbConn = dbConnStrategy.GetConnStr();
                IDbContextFactory<ReservDbContext> dbContext = new DbContextFactory(dbConn);

                builder.Services.AddSingleton(dbContext);
                builder.Services.AddSingleton<IReserveRepository, PostgresSqlReserveRepository>();
                builder.Services.AddSingleton<IRoomRepository, PostgresSqlRoomRepository>();
                builder.Services.AddSingleton<IReservesCalendarService, ReservesCalendarService>();
                builder.Services.AddSingleton<IReserveService, ReserveService>();
                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

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

                var app = builder.Build();

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
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
