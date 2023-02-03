using Microsoft.AspNetCore.Mvc;

namespace BookShop.Mvc.Controllers
{
    public class CardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
