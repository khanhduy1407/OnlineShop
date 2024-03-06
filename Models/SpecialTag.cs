using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên thẻ sản phẩm")]
        [Display(Name = "Thẻ sản phẩm")]
        public string Name { get; set; }
    }
}
