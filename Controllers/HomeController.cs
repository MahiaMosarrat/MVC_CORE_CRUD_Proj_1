using Microsoft.AspNetCore.Mvc;

namespace CoreFinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
