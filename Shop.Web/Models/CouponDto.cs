using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        [Required]
        public int MinAmount { get; set; }
    }
}
