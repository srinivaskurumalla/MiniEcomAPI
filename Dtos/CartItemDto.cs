using MiniEcom.Api.Dtos;

namespace MiniEcom.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }

        public ProductDto Product { get; set; } = new();
    }
}
