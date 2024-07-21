using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP.NET_BookShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var user_id = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(cart => cart.UserId == user_id, includeProperties: "Product")
            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var cart_from_database = _unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.Id == cartId);
            cart_from_database.Count += 1;
            _unitOfWork.ShoppingCart.Update(cart_from_database);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Book Added";

            return RedirectToAction("Index", "ShoppingCart");
        }
        public IActionResult Minus(int cartId)
        {
            var cart_from_database = _unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.Id == cartId);
            if (cart_from_database.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart_from_database);
                TempData["success"] = "Books Removed";
            }
            else
            {
                cart_from_database.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cart_from_database);
                TempData["success"] = "Book Removed";
            }
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index", "ShoppingCart");
        }
        public IActionResult Remove(int cartId)
        {
            var cart_from_database = _unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart_from_database);
            _unitOfWork.SaveChanges();
            TempData["success"] = "All Books Removed";

            return RedirectToAction("Index", "ShoppingCart");
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shopping_cart)
        {
            switch (shopping_cart.Count)
            {
                case <= 50:
                    return shopping_cart.Product.Price;
                case <= 100:
                    return shopping_cart.Product.Price50;
                case > 100:
                    return shopping_cart.Product.Price100;
            }
        }
    }
}
