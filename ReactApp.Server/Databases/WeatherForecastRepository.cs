using Dapper;
using Microsoft.Data.SqlClient;
using ReactApp.Server.Interfaces;

namespace ReactApp.Server.Databases
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private static readonly string ConnectionString = @"Server=(localdb)\datapoint;Database=master;Integrated Security=True;TrustServerCertificate=True;";


        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT [Date], TemperatureC, TemperatureF, Summary FROM dbo.weatherfast";
                return await connection.QueryAsync<WeatherForecast>(sql);
            }
        }
    }
}

    