using MiniEcom.Api.Dtos;

namespace MiniEcom.Dtos
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderPlacedAt { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

        public AddressDto ShippingAddress { get; set; }
        public AddressDto? BillingAddress { get; set; }

        public List<OrderItemDetailsDto> Items { get; set; } = new();
    }

    public class OrderItemDetailsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}
