using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name = "Mã đơn hàng")]
        public int OrderId { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Products Product { get; set; }
    }
}
