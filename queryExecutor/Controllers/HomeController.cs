using System;
using System.Reflection;
using System.Web.Mvc;

namespace queryExecutor.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Version = Assembly.GetExecutingAssembly().GetName().Version;
            return View();
        }
    }
}