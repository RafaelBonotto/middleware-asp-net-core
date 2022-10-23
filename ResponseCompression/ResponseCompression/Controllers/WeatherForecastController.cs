using Microsoft.AspNetCore.Mvc;
using ResponseCompression.FilterMiddleware;

namespace ResponseCompression.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[MiddlewareFilter(typeof(ChecagemIndisponibilidadePipeline))]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [MiddlewareFilter(typeof(CompressaoGZipPipeline))]
        public IEnumerable<WeatherForecast> Get()
        {
            throw new Exception("TESTE");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}