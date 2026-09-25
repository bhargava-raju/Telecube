using Microsoft.AspNetCore.Mvc;
using ReactApp.Server.Interfaces;

namespace ReactApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(IWeatherForecastService weatherForecastService) 
        {
            _weatherForecastService = weatherForecastService;              
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast()
        {
            return await _weatherForecastService.GetWeatherForecast();
        }
    }
}
