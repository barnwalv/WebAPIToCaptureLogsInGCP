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

        // Constructor to initialize GoogleLogger
        public WeatherForecastController(GoogleLogger googleLogger)
        {
            _googleLogger = googleLogger;
            _googleLogger.LoggedToGoogleLog(LogSeverity.Info, "WeatherForecastController contstructor initialized"); // Capturing Info Log type to GCP Log Explorer, when constructor is initialized.
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };



        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _googleLogger.LoggedToGoogleLog(LogSeverity.Info, "GetWeatherForecast called"); // Capturing Info Log type to GCP Log Explorer, whem method is called.
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
