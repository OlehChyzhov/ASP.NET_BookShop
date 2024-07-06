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
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString()) ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            if (ModelState.IsValid == false || category.Name == null) return View();

            shop_context.Categories.Add(category);
            shop_context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category category_from_database = shop_context.Categories.Find(id)!;
            if (category_from_database == null)
            {
                return NotFound();
            }
            return View(category_from_database);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString()) ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            if (ModelState.IsValid == false || category.Name == null) return View();

            shop_context.Update(category);
            shop_context.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        public IActionResult Delete(int? id)
        {
            Category? category_from_database = shop_context.Categories.Find(id);
            if (category_from_database == null) return NotFound();
            
            shop_context.Remove(category_from_database);
            shop_context.SaveChanges();

            return RedirectToAction("Index", "Category");
        }
    }
}
