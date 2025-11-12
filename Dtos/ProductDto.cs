using MiniEcom.Dtos;

namespace MiniEcom.Api.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string shortDescription { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = [];
    }
}
