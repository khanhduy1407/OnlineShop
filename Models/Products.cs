using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineShop.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Thêm giá cho sản phẩm")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Image {  get; set; }
        [Display(Name = "Màu sắc")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name = "Có sẵn")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Loại sản phẩm")]
        [Required]
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }

        [Display(Name = "NSX")]
        [Required]
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag SpecialTag { get; set; }

        [Display(Name = "Ngày đăng")]
        public DateTime CreatedAt { get; set; }
    }
}
