using Customers.Core.Data;
using Customers.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;

namespace Customers.Core
{
    public class EfClass
    {


        /* Только для обучающего проекта
         * Developers settings .env
         # Database Configuration
        DB_HOST=host.docker.internal
        DB_DATABASE=EfCoreDb
        DB_USERNAME=postgres
        DB_PASSWORD=postgres
        DB_PORT_EXTERNAL=15432
        DB_PORT_INTERNAL=5432
        DB_DATA_VOLUME=postgres_data
         */
        //docker-compose up -d
        //docker-compose down -v

        public static string GetConnectionString()
        {
            // Загрузка .env файла
            Env.Load();
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT_EXTERNAL");
            var user = Environment.GetEnvironmentVariable("DB_USERNAME");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var database = Environment.GetEnvironmentVariable("DB_DATABASE");

            return
                            $"Host={host};Port={port};" +
                            $"Username={user};Password={password};" +
                            $"Database={database};";

        }

        public static void InitDatabase()
        {
            var connectionString = GetConnectionString();

            // Загрузка конфигурации
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Строка подключения к PostgreSQL в Docker
            //var connectionString = configuration.GetConnectionString("PostgresConnection");

            // Настройка DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using var context = new AppDbContext(optionsBuilder.Options);

            try
            {
                // 1. Создаем базу данных (если не существует)
                Console.WriteLine("Создание базы данных...");
                context.Database.EnsureCreated();
                Console.WriteLine("База данных создана!");

                // 2. Добавляем тестовые данные
                Console.WriteLine("\nДобавление пользователей...");

                var Customers = new[]
                {
                new Customer { FullName = "Иван Иванов", Email = "ivan@example.com", Phone = "+79161234561", CreatedAt = DateTime.UtcNow },
                new Customer { FullName = "Мария Петрова", Email = "maria@example.com", Phone = "+79161234562", CreatedAt = DateTime.UtcNow },
                new Customer { FullName = "Алексей Сидоров", Email = "alex@example.com", Phone = "+79161234563", CreatedAt = DateTime.UtcNow }
            };

                context.Customers.AddRange(Customers);
                context.SaveChanges();
                Console.WriteLine("Пользователи добавлены!");

                // 3. Читаем данные из базы
                Console.WriteLine("\nСписок пользователей в базе данных:");
                var allUsers = context.Customers.ToList();

                foreach (var _user in allUsers)
                {
                    Console.WriteLine($"ID: {_user.Id}, Имя: {_user.FullName}, Email: {_user.Email}, Создан: {_user.CreatedAt}");
                }

                // 4. Получаем количество пользователей
                var userCount = context.Customers.Count();
                Console.WriteLine($"\nВсего пользователей в базе: {userCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
                }
            }
        }


    }
}
