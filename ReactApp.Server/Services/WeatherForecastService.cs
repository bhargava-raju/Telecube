using ReactApp.Server.Interfaces;

namespace ReactApp.Server.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private IWeatherForecastRepository _weatherForecastRepository;
        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }
       
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            return await _weatherForecastRepository.GetWeatherForecast();
        }
    }
}
