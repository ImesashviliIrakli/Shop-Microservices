using Shop.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

        [Range(1,100)]
        public int Count { get; set; } = 1;
        public string ImageLocalPath { get; set; }
        [AllowedExtensions(new string[] {".jpg",".png"})]
        public IFormFile Image { get; set; }

    }
}
