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
        [Required(ErrorMessage = "Nhập tên nhà sản xuất")]
        [Display(Name = "Nhà sản xuất")]
        public string Name { get; set; }
    }
}
