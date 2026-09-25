using ReactApp.Server.Interfaces;
using ReactApp.Server.Services;
using ReactApp.Server.Databases;
namespace ReactApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs / mapping endpoints
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            builder.Services.AddTransient<IWeatherForecastRepository, WeatherForecastRepository>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();   
                app.UseSwaggerUI(); 

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
