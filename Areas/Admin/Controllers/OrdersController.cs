using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? type)
        {
            var orders = string.IsNullOrEmpty(type) ? _db.Orders.ToList() : _db.Orders.Where(o => o.Status.ToString() == type.ToString()).ToList();
            return View(orders);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string userEmail = User.Identity.Name;

            var order = _db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .Where(o => o.Id == id)
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }
            var items = new List<string> { "Đã đặt hàng", "Đang giao", "Đã nhận", "Đã hủy", "Không nhận" };
            ViewBag.ListStatus = new SelectList(items);
            return View(order);
        }

        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> UpdateOrderStatus(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
