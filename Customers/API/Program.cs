
using Customers.Core;
using Customers.Core.Data;
using Microsoft.EntityFrameworkCore;


namespace Customers.API
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            //var initializer = new ApplicationInitializer();
            //docker-compose up -d
            //docker-compose down -v
            //initializer.InitializeApplicationAsync();
            EfClass.InitDatabase();


            var builder = WebApplication.CreateBuilder(args);
            // 1. Добавьте эту строку для регистрации DbContext
            // Регистрация DbContext для PostgreSQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    //builder.Configuration.GetConnectionString("PostgresConnection"),
                    EfClass.GetConnectionString(),
                    npgsqlOptions =>
                    {
                        // Дополнительные настройки
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorCodesToAdd: null);

                        // Включение поддержки диапазонов (range) и других типов
                        //npgsqlOptions.UseNodaTime();
                    }));

            // Add services to the container.

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

            // 6. Создание БД при запуске (опционально)
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    dbContext.Database.EnsureCreated(); // или dbContext.Database.Migrate();
            //}

            app.Run();
        }
    }
}
