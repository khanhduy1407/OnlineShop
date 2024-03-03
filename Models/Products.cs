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
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image {  get; set; }
        [Display(Name = "Màu sản phẩm")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name = "Có sẵn")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Loại sản phẩm")]
        [Required]
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }

        [Display(Name = "Thẻ")]
        [Required]
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag SpecialTag { get; set; }
    }
}
