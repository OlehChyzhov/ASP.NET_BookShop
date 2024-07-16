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
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IUnitOfWork unit, IWebHostEnvironment environment)
        {
            unitOfWork = unit;
            webHostEnvironment = environment;
        }
        public IActionResult Index()
        {
            List<Product> all_products = unitOfWork.Product.GetAll("Category").ToList();
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
            // Validation
            if (view_model.Product.Title == view_model.Product.Author) ModelState.AddModelError("Title", "Title cannot exactly match the author");
            if (ModelState.IsValid == false) return View();

            // File image saving
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (File != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\products");

                if (string.IsNullOrEmpty(view_model.Product.ImageUrl) == false)
                {
                    // Delete old image
                    var old_image_path = Path.Combine(wwwRootPath, view_model.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(old_image_path))
                    {
                        System.IO.File.Delete(old_image_path);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    File.CopyTo(fileStream);
                }
                view_model.Product.ImageUrl = @"\images\products\" + fileName;
            }

            // Product creating
            if (view_model.Product.Id == 0)
            {
                unitOfWork.Product.Add(view_model.Product);
                TempData["success"] = "Product created successfully";
            }
            // Product updating
            else
            {
                unitOfWork.Product.Update(view_model.Product);
                TempData["success"] = "Product updated successfully";
            }
            unitOfWork.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> all_products = unitOfWork.Product.GetAll("Category").ToList();
            return Json(new { data = all_products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product_from_database = unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id);
            if (product_from_database == null) return Json(new { success = false, message = "No such product exists" });

            var oldImagePath = product_from_database.ImageUrl;

            // Delete old image
            var old_image_path = Path.Combine(webHostEnvironment.WebRootPath, product_from_database.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(old_image_path))
            {
                System.IO.File.Delete(old_image_path);
            }

            unitOfWork.Product.Remove(product_from_database);
            unitOfWork.SaveChanges();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
