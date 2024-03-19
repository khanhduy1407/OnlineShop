using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Marketing")]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        //[AllowAnonymous]
        public IActionResult Index()
        {
            // var data = _db.ProductTypes.ToList();
            return View(_db.ProductTypes.ToList());
        }

        // GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        // POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["message"] = "Đã thêm loại sản phẩm mới";
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        // GET Edit Action Method
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["message"] = $"Đã cập nhật loại sản phẩm.";
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }

        // GET Details Action Method
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST Details Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET Delete Action Method
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes productTypes)
        {
            if (id==null)
            {
                return NotFound();
            }

            if (id != productTypes.Id)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var temp = _db.ProductTypes.Find(productTypes.Id).ProductType;
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["remove"] = $"Đã xóa loại sản phẩm {temp}";
                return RedirectToAction(nameof(Index));
            }

            return View(productTypes);
        }
    }
}
