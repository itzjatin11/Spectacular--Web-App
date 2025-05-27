using Microsoft.AspNetCore.Mvc;

namespace SpecTacular.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            string message = statusCode switch
            {
                401 => "Authorization required. You need to login to access this page.",
                403 => "Access denied. You don’t have permission to view this resource.",
                404 => "Oops! The page you're looking for doesn't exist.",
                _ => "Something went wrong. Please try again later."
            };

            ViewBag.StatusCode = statusCode;
            ViewBag.Message = message;

            return View("Index");
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            ViewBag.StatusCode = 500;
            ViewBag.Message = "Internal server error. We're working on it!";

            return View("Index");
        }

        public IActionResult CauseError()
        {
            throw new Exception("This is a test exception!");
        }

    }

}
