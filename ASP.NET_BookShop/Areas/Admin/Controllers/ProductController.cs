using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product.Title == product.Author) ModelState.AddModelError("Title", "Title cannot exactly match the author");
            if (ModelState.IsValid == false) return View();
            unitOfWork.Product.Add(product);
            unitOfWork.SaveChanges();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index", "Product");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product? product_form_database = unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id);
            return View(product_form_database);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (product.Title == product.Author) ModelState.AddModelError("Title", "Title cannot exactly match the author");
            if (ModelState.IsValid == false) return View();
            unitOfWork.Product.Update(product);
            unitOfWork.SaveChanges();
            TempData["success"] = "Product updated successfully";
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
