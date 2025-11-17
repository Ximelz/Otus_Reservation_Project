using Hotels.Domain.src.Repositories;
using Hotels.Domain.src.Services;
using Hotels.Domain.src.Entities;
using Hotels.Infrastructure.Pg.src.Services;
using Hotels.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;


namespace Hotels.Infrastructure
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlDatabaseContext>();

            var options = optionsBuilder
                    .UseNpgsql(GetConnectionString())
                    .Options;

            SqlDatabaseContext sqlDbContext = new(options);

            IHotelsRepository hotelsRepository = new HotelsSqlRepository(sqlDbContext);
            IHotelsService hotelsService = new HotelsService(hotelsRepository);

            Hotel newHotel = new();
            newHotel.Name = "Astoria";
            newHotel.Stars = 5;
            newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            newHotel.CountryId = 1;
            newHotel.Phone = "+74924123377";
            newHotel.Email = "service@gh-astoria-hotel.ru";

            //await hotelsService.Add(newHotel);

            //var hotels = await hotelsService.GetAllByStars([5]);

            //foreach (var hotel in hotels)
            //{
            //    Console.WriteLine($"saved hotel: {hotel.Name}");
            //}

            
        }

        static private string GetConnectionString()
        {
            string connectionString = "";

            using (StreamReader r = new StreamReader(".\\DbConfiguration.json"))
            {
                string jsonString = r.ReadToEnd();
                JsonNode rootNode = JsonNode.Parse(jsonString)!;
                JsonNode connectionNode = rootNode!["ConnectionString"]!;
                connectionString = connectionNode.ToString();
            }
            
            return connectionString;
        }
    }
}
