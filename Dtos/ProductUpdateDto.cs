using System.ComponentModel.DataAnnotations;

namespace MiniEcom.Dtos
{
    public class ProductUpdateDto
    {
        [Required]
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        [Required, MaxLength(500)]
        public string Name { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal? Mrp { get; set; }
        public decimal TaxPercent { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        // Optional — to replace or add new images
        public List<IFormFile>? Images { get; set; }

        // Optional — new tags (comma separated in UI)
        public List<string>? Tags { get; set; }
    }

}
