using Customers.Core.Data;
using Customers.Core.DTOs;
using Customers.Core.Interfaces;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.PostgreSQL
{
    public class PostgreEfClass: IDBEFClass
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

        public Action<DbContextOptionsBuilder> ConfigureDbContextOptions()
        {
            return options => options.UseNpgsql(
                GetConnectionString(),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                    // Включение поддержки диапазонов (range) и других типов
                    //npgsqlOptions.UseNodaTime();
                });
        }


        public string GetConnectionString()
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

        public bool CreateDB(PostgreAppDbContext context)
        {
            Console.WriteLine("Создание базы данных...");
            var res = context.Database.EnsureCreated();
            Console.WriteLine("База данных создана!");
            return res;
        }

        public void AddTestData(PostgreAppDbContext context)
        {
            // 2. Проверяем, есть ли уже записи в таблице Customers
            /*Console.WriteLine("\nПроверка наличия данных в таблице Customers...");
            bool hasExistingData = context.Customers.Any();

            //Добавляем тестовые данные
            if (hasExistingData)
            {
                Console.WriteLine("В таблице Customers уже есть данные. Пропускаем добавление тестовых данных.");
            }
            else*/
            {
                Console.WriteLine("\nДобавление постояльцев...");

                var Customers = new[]
                {
                    new Customer { FullName = "Иван Иванов", Email = "ivan@example.com", Phone = "+79161234561", CreatedAt = DateTime.UtcNow },
                    new Customer { FullName = "Мария Петрова", Email = "maria@example.com", Phone = "+79161234562", CreatedAt = DateTime.UtcNow },
                    new Customer { FullName = "Алексей Сидоров", Email = "alex@example.com", Phone = "+79161234563", CreatedAt = DateTime.UtcNow }
                };

                context.Customers.AddRange(Customers);
                context.SaveChanges();
                Console.WriteLine("Постояльцы добавлены!");
            }
        }

        public void PrintTestData(PostgreAppDbContext context)
        {
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

        public PostgreAppDbContext GetContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreAppDbContext>();
            optionsBuilder.UseNpgsql(GetConnectionString());

            return new PostgreAppDbContext(optionsBuilder.Options);
        }

        public void EnsureCreatedDatabase()
        {
            // Загрузка конфигурации
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            using var context = GetContext();
            try
            {
                // 1. Создаем базу данных (если не существует)
                bool hasExistingData = CreateDB(context);
                if (hasExistingData)
                // 2. Добавляем тестовые данные
                AddTestData(context);

                // 3. Читаем данные из базы
                PrintTestData(context);
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
