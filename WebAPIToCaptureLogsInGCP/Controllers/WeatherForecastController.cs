using Google.Cloud.Logging.Type;
using GoogleLogs4DotNet;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIToCaptureLogsInGCP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly GoogleLogger _googleLogger;
        //private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(GoogleLogger googleLogger)
        {
            _googleLogger = googleLogger;
            _googleLogger.LoggedToGoogleLog(LogSeverity.Info, "WeatherForecastController contstructor initialized");
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };



        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
