using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace GatewayAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddOcelot("ocelot.json");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOcelot(builder.Configuration);

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseOcelot().Wait();

            app.MapControllers();
            app.Run();
        }
    }
}
