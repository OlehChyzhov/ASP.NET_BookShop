using ASP.NET_BookShop.Data;
using ASP.NET_BookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_BookShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ShopContext shop_context;
        public CategoryController(ShopContext context)
        {
            shop_context = context;
        }
        public IActionResult Index()
        {
            List<Category> all_categories = shop_context.Categories.ToList();
            return View(all_categories);
        }
    }
}
