using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên loại sản phẩm")]
        [Display(Name = "Loại sản phẩm")]
        public string ProductType { get; set; }
    }
}
