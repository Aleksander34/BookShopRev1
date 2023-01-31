using Microsoft.AspNetCore.Mvc;

namespace BookShop.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
