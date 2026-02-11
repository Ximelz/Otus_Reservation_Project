
using Core.Data.PostgreSQL;
using Core.Data;
using Customers.Core.Interfaces;
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

            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddScoped<IDBEFClass, PostgreEfClass>();

            IDBEFClass EfClass = new PostgreEfClass();
            // Создание БД при запуске (опционально)
            EfClass.EnsureCreatedDatabase();
            // Регистрация DbContext для PostgreSQL
            builder.Services.AddDbContext<PostgreAppDbContext>(EfClass.ConfigureDbContextOptions());


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

            app.Run();
        }
    }
}
