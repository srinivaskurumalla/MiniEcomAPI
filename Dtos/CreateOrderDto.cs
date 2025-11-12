using System.ComponentModel.DataAnnotations;

namespace MiniEcom.Api.Dtos
{
    public class OrderItemDto
    {
        [Required] public int ProductId { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public decimal UnitPrice { get; set; }
    }

    public class CreateOrderDto
    {
        [Required] public int UserId { get; set; }
        [Required] public int ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }
        [Required] public OrderItemDto[] Items { get; set; }
    }
}
