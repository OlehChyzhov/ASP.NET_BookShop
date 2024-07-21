using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ASP.NET_BookShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            unitOfWork = unit;
        }

        public IActionResult Index()
        {
            List<Product> products = unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }
        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id, "Category")!,
                Count = 1,
                ProductId = id,
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shopping_cart)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var user_id = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            shopping_cart.UserId = user_id;

            ShoppingCart? cart_from_database = unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.UserId == user_id && 
            cart.ProductId == shopping_cart.ProductId);

            if (cart_from_database != null)
            {
                // shopping cart exists
                cart_from_database.Count += shopping_cart.Count;
                unitOfWork.ShoppingCart.Update(cart_from_database);
                TempData["success"] = "Your order has been updated";
            }
            else
            {
                // add cart record
                unitOfWork.ShoppingCart.Add(shopping_cart);
                TempData["success"] = "Your order has been registered";
            }
            unitOfWork.SaveChanges();
            

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
