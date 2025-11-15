namespace MiniEcom.Dtos
{
    public class CheckoutDto
    {
        public int ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }
        public string PaymentMethod { get; set; } = "COD";
        public string? Notes { get; set; }
    }
}
