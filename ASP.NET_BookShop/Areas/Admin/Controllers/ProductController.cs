using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }
        public IActionResult Index()
        {
            List<Product> all_products = unitOfWork.Product.GetAll().ToList();
            return View(all_products);
        }
        public IActionResult UpdateOrInsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = unitOfWork.Category.GetAll().Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()
            });
            ProductVM productVM = new() { CategoryList = CategoryList, Product = new Product() };
            
            if (id == null) return View(productVM);
            else
            {
                productVM.Product = unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id)!;
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult UpdateOrInsert(ProductVM view_model, IFormFile? File)
        {
            if (view_model.Product.Title == view_model.Product.Author) ModelState.AddModelError("Title", "Title cannot exactly match the author");
            if (ModelState.IsValid == false) return View();
            unitOfWork.Product.Add(view_model.Product);
            unitOfWork.SaveChanges();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index", "Product");
        }
        public IActionResult Delete(int? id)
        {
            Product? product_from_database = unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id);
            if (product_from_database == null) return NotFound();

            unitOfWork.Product.Remove(product_from_database);
            unitOfWork.SaveChanges();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index", "Product");
        }
    }
}
