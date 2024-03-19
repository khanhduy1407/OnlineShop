using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _db;

        public OrderController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            // Get the user's email from the authentication system
            string userEmail = User.Identity.Name;

            // Query the database for orders associated with the user's email
            var userOrders = _db.Orders
                .Where(o => o.Email.ToLower() == userEmail.ToLower())
                .ToList();

            return View(userOrders);
        }

        // GET Checkout action method
        public IActionResult Checkout()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.UserEmail = User.Identity.Name;

            var user = _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            ViewBag.FullName = $"{user.FirstName} {user.LastName}";
            ViewBag.PhoneNumber = user.PhoneNumber == null ? "" : user.PhoneNumber;

            return View();
        }

        // POST Checkout action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }

            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Products>());

            TempData["message"] = $"Bạn đã đặt hàng thành công với mã đơn hàng #{GetOrderNo()}";

            return RedirectToAction(nameof(Index));
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

        public IActionResult Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userEmail = User.Identity.Name;

            var order = _db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .Where(o => o.Id.ToString() == id.ToString() && o.Email.ToLower() == userEmail.ToLower())
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
