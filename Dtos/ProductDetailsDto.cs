namespace MiniEcom.Dtos
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? LongDescription { get; set; }

        public decimal Price { get; set; }

        public decimal? Mrp { get; set; }

       // public decimal TaxPercent { get; set; }

        public int StockQuantity { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = [];
    }
}
