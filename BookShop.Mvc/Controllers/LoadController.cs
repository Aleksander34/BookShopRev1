using Microsoft.AspNetCore.Mvc;

namespace BookShop.Mvc.Controllers
{
    public class LoadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
