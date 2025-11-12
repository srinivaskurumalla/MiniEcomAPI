using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int UserId { get; set; }

    public int ShippingAddressId { get; set; }

    public int? BillingAddressId { get; set; }

    public DateTime OrderPlacedAt { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal ShippingCharge { get; set; }

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; } = null!;

    public bool IsPaid { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentId { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public virtual Address? BillingAddress { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Address ShippingAddress { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
