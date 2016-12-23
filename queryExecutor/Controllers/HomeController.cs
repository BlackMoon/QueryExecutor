using System.Web.Mvc;

namespace queryExecutor.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return Content("Home page");
        }
    }
}