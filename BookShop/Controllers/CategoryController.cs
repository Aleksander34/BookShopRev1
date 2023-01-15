using BookShop.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class CategoryController : BookShopController
    {
        private readonly BookShopContext _context;
        public IActionResult GetCategoryChart()
        {
            var result = _context.Books.GroupBy(x => x.Category).Select(x => new { Category = x.Key, Count=x.Count()}).ToList();
            return Ok(result);
        }
    }
}
