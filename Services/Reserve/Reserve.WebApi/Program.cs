namespace ReservService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dbConn = "Host=localhost;Database=ReserveService;Username=postgres;Password=12345;Port=5432";

            var builder = WebApplication.CreateBuilder();

            IDbContextFactory<ReservDbContext> dbContext = new DbContextFactory(dbConn);

            builder.Services.AddSingleton(dbContext);
            builder.Services.AddSingleton<IReserveRepository, PostgresSqlReserveRepository>();
            builder.Services.AddSingleton<IReserveService, ReserveService>();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
    }
}
