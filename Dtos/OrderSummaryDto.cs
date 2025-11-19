using MiniEcom.Api.Dtos;

namespace MiniEcom.Dtos
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderPlacedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public bool IsPaid { get; set; }
        public List<OrderSummaryItemDto> Items { get; set; } = new();
    }
    public class OrderSummaryItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
