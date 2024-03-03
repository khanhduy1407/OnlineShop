using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }

        public int Id { get; set; }
        [Display(Name = "Mã đơn hàng")]
        public string OrderNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Số điện thoại")]
        public string PhoneNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
