using Google.Cloud.Logging.Type;
using GoogleLogs4DotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIToCaptureLogsInGCP.ActionFilter;

namespace WebAPIToCaptureLogsInGCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly GoogleLogger _googleLogger;

        public EmployeeController(GoogleLogger googleLogger)
        {
            _googleLogger = googleLogger;
        }

        //method to show dummy employee data
        [HttpGet]
        [ServiceFilter(typeof(LogActionFilter))]
        public IActionResult GetEmployees()
        {
            //_googleLogger.LoggedToGoogleLog(LogSeverity.Info, "GetEmployees method called"); // Capturing Info Log type to GCP Log Explorer, when method is called.

            var employees = new List<object>
            {
                new { Id = 1, Name = "John Doe", Position = "Software Engineer" },
                new { Id = 2, Name = "Jane Smith", Position = "Project Manager" },
                new { Id = 3, Name = "Sam Brown", Position = "QA Analyst" }
            };

            return Ok(employees);
        }

        //method to throw some exception
        [HttpGet("error")]
        [ServiceFilter(typeof(LogActionFilter))]
        public IActionResult GetError()
        {
            throw new Exception("This is a test exception to demonstrate error logging.");
        }
    }
}
