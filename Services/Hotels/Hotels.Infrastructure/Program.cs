using Hotels.Domain.Repositories;
using Hotels.Domain.Services;
using Hotels.Domain.Entities;

using Hotels.Infrastructure.Services;
using Hotels.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;


namespace Hotels.Infrastructure
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            PgDbContextOptions pgOptionsBuilder = new(".\\DbConfiguration.json");

            IHotelsRepository hotelsRepository = new HotelsSqlRepository(pgOptionsBuilder.GetOptions());
            IHotelsService hotelsService = new HotelsService(hotelsRepository);

            //Hotel newHotel = new();
            //newHotel.Name = "Astoria";
            //newHotel.Stars = 5;
            //newHotel.Address = "Gus Hrustalniy, Central Street, 177";
            //newHotel.CountryId = 1;
            //newHotel.Phone = "+74924123377";
            //newHotel.Email = "service@gh-astoria-hotel.ru";

            //await hotelsService.Add(newHotel);

            var hotels = await hotelsService.GetAllByStars([5]);
            Hotel firstHotel = hotels.First();

            Console.WriteLine($"Hotels count: {hotels.Count}");
            Console.WriteLine($"First hotel id: {firstHotel.Id}");

            await hotelsService.Remove(firstHotel.Id);
            Console.WriteLine("removing...");

            hotels = await hotelsService.GetAllByStars([5]);
            firstHotel = hotels.First();

            Console.WriteLine($"Hotels count: {hotels.Count}");
            Console.WriteLine($"First hotel id: {firstHotel.Id}");

            //foreach (var hotel in hotels)
            //{
            //    Console.WriteLine($"saved hotel: {hotel.Name}");
            //}


        }

        static private string GetConnectionPassword()
        {
            string password = "";

            using (StreamReader r = new StreamReader(".\\DbConfiguration.json"))
            {
                string jsonString = r.ReadToEnd();
                JsonNode rootNode = JsonNode.Parse(jsonString)!;
                JsonNode connectionNode = rootNode!["ConnectionData"]!;

                if (connectionNode != null)
                {
                    password = connectionNode!["Password"]!.ToString();
                }
            }
            
            return password;
        }
    }
}
