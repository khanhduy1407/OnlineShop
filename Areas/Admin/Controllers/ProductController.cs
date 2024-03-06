using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(c=>c.ProductTypes).Include(f=>f.SpecialTag).ToList());
        }

        // Post Index Action Method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList();
            }
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var fileUrl = await UploadFileToS3(file);
                TempData["message"] = "Đã upload hình ảnh lên server.";
                return Json(new { fileUrl });
            }
            catch (Exception ex)
            {
                // Handle errors
                return Json(new { error = ex.Message });
            }
        }

        public async Task<string> UploadFileToS3(IFormFile file)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "-" + file.FileName;
            var fileUrl = "https://wwgrocery.s3-ap-southeast-1.amazonaws.com/" + fileName;

            var awsAccessKeyId = "AKIA42T6RAMBQPKEOIGB";
            var awsSecretAccessKey = "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";
            var awsRegionEndpoint = Amazon.RegionEndpoint.APSoutheast1;
            var clientConfig = new AmazonS3Config
            {
                RegionEndpoint = awsRegionEndpoint
            };

            try
            {
                using (var awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, clientConfig))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        var request = new TransferUtilityUploadRequest
                        {
                            InputStream = memoryStream,
                            Key = fileName,
                            BucketName = "wwgrocery",
                            CannedACL = S3CannedACL.PublicRead,
                            ContentType = file.ContentType,
                        };
                        var transferUtility = new TransferUtility(awsS3Client);
                        await transferUtility.UploadAsync(request);
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                // Handle S3-specific exceptions
                Console.WriteLine("Error uploading to S3: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("Error uploading: " + ex.Message);
                throw;
            }
            return fileUrl;
        }

        // Get Create Method
        public IActionResult Create()
        {
            ViewBag.productTypeList = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewBag.tagList = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            return View();
        }

        // Post Create Method
        [HttpPost]
        public async Task<IActionResult> Create(Products product)
        {
            var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
            if (searchProduct != null)
            {
                ViewBag.message = "Sản phẩm này đã tồn tại";
                ViewBag.productTypeList = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                ViewBag.tagList = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                return View(product);
            }

            product.CreatedAt = DateTime.Now;

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Đã thêm sản phẩm \"{product.Name}\" vào kệ hàng.";
            return RedirectToAction(nameof(Index));
        }

        // Get Edit Method
        public IActionResult Edit(int? id)
        {
            ViewBag.productTypeList = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewBag.tagList = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Post Edit Method
        [HttpPost]
        public async Task<IActionResult> Edit(Products product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            TempData["message"] = $"Đã cập nhật sản phẩm \"{product.Name}\" thành công.";
            return RedirectToAction(nameof(Index));
        }

        // Get Details Method
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Get Delete Method
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Post Delete Method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["remove"] = $"Đã xóa sản phẩm \"{product.Name}\" khỏi kệ hàng.";
            return RedirectToAction(nameof(Index));
        }
    }
}
