using Microsoft.AspNetCore.Mvc;

namespace BookShop.Mvc.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
