using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MiniEcom.Dtos
{
    public class ProductCreateDto
    {
        public int? CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string SKU { get; set; }

        [Required, MaxLength(500)]
        public string Name { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal? MRP { get; set; }
        public decimal TaxPercent { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        // For file uploads
        public List<IFormFile>? Images { get; set; }

        // NEW → Admin provides tags like: “makeup, eye, beauty”
        public List<string>? Tags { get; set; }
    }
}
