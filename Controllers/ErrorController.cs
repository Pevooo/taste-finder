
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TasteFinder.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                var exception = exceptionHandlerPathFeature.Error;
                // Log the exception details (example)
                Console.WriteLine($"Exception: {exception.Message}");
                Console.WriteLine($"Path: {exceptionHandlerPathFeature.Path}");
            }

            return View("~/Views/Shared/Error.cshtml"); // Return the view from the Shared folder
        }
    }
}