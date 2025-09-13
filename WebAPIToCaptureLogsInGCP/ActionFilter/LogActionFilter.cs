using Google.Cloud.Logging.Type;
using GoogleLogs4DotNet;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIToCaptureLogsInGCP.ActionFilter
{
    public class LogActionFilter: ActionFilterAttribute
    {
        private readonly GoogleLogger _googleLogger;
        public LogActionFilter(GoogleLogger googleLogger)
        {
            _googleLogger = googleLogger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Code to execute before the action executes.
            // You can add logging or other pre-processing logic here.
            // For example, log the action name, is  user is Authenticated or not, etc...:
            var httpContext = context.HttpContext;
            httpContext.Items["StartTime"] = DateTime.UtcNow; // store start time for duration calc

            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];
            var method = httpContext.Request.Method;
            var path = httpContext.Request.Path;
            var user = httpContext.User.Identity?.Name ?? "Anonymous";
            var parameters = string.Join(", ", context.ActionArguments.Select(kv => $"{kv.Key}={kv.Value}"));
            bool isUserIsAuthenticated = httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated;

            var logMessage =
                        $"[START REQUEST (controller/action) - {controller}/{action}]\n" +
                        $"Method: {method}\n" +
                        $"Path: {path}\n" +
                        $"User: {user}\n" +
                        $"Params: {parameters}\n" +
                        $"Timestamp: {DateTime.UtcNow:O}\n" +
                        $"IsUserAuthenticated: {isUserIsAuthenticated}";

            //if in release mode log to GCP log explorer
            _googleLogger.LoggedToGoogleLog(Google.Cloud.Logging.Type.LogSeverity.Info, logMessage);

            //Uncomment during local debug mode.
            // You can replace this with _googleLogger.LoggedToGoogleLog(...)
            //Console.WriteLine(logMessage);

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Code to execute after the action executes.
            // You can add logging or other post-processing logic here.

            var httpContext = context.HttpContext;

            var startTime = httpContext.Items["StartTime"] != null
                ? (DateTime)httpContext.Items["StartTime"]
                : DateTime.UtcNow;

            var duration = DateTime.UtcNow - startTime;

            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];
            var statusCode = httpContext.Response?.StatusCode;
            var exception = context.Exception;

            var logMessage =
                $"[END REQUEST (controller/action) - {controller}/{action}]\n" +
                $"StatusCode: {statusCode}\n" +
                $"Exception: {(exception != null ? exception.Message : "None")}\n" +
                $"Duration: {duration.Seconds} s\n" +
                $"Timestamp: {DateTime.UtcNow:O}";

            if (exception != null)
            {
                // Log with Error severity
                _googleLogger.LoggedToGoogleLog(LogSeverity.Error, logMessage);
            }
            else
            {
                // Log with Info severity
                _googleLogger.LoggedToGoogleLog(LogSeverity.Info, logMessage);
            }
            base.OnActionExecuted(context);
        }
    }
}
