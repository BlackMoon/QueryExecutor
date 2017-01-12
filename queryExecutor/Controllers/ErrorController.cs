using System;
using System.Net;
using System.Web.Mvc;
using queryExecutor.Domain;

namespace queryExecutor.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        protected override void HandleUnknownAction(string code)
        {
            int statusCode;
            int.TryParse(code, out statusCode);

            ErrorMessage msg = new ErrorMessage()
            {
                Code = statusCode,
                Description = Enum.GetName(typeof(HttpStatusCode), statusCode)
            };

            View("Error", msg).ExecuteResult(ControllerContext);

            Response.StatusCode = statusCode;
        }

        // Handle 401: Unauthorized
        public ActionResult Unauthorized()
        {
            ErrorMessage msg = new ErrorMessage()
            {
                Code = 401,
                Description = Enum.GetName(typeof(HttpStatusCode), 401)
            };

            return View("Error", msg);
        }
    }
}