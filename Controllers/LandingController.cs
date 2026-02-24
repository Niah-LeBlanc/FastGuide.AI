using Microsoft.AspNetCore.Mvc;

namespace Lecture2Guide.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
