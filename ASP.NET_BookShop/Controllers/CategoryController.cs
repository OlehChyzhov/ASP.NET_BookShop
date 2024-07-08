﻿using BookShop.DataAccess.Data;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using BookShop.DataAccess.Repository.IRepository;

namespace ASP.NET_BookShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository catetoryRepo)
        {
            categoryRepository = catetoryRepo;
        }
        public IActionResult Index()
        {
            List<Category> all_categories = categoryRepository.GetAll().ToList();
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

            categoryRepository.Add(category);
            categoryRepository.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
     
            Category? category_from_database = categoryRepository.GetFirstOrDefault(category => category.Id == id);

            if (category_from_database == null) return NotFound();
            return View(category_from_database);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString()) ModelState.AddModelError("name", "The Display Order cannot exactly match the Name");
            if (ModelState.IsValid == false || category.Name == null) return View();

            categoryRepository.Update(category);
            categoryRepository.SaveChanges();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index", "Category");
        }
        public IActionResult Delete(int? id)
        {
            Category? category_from_database = categoryRepository.GetFirstOrDefault(category => category.Id == id);
            if (category_from_database == null) return NotFound();
            
            categoryRepository.Remove(category_from_database);
            categoryRepository.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
