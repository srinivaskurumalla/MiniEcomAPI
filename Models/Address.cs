using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Label { get; set; }

    public string RecipientName { get; set; } = null!;

    public string Line1 { get; set; } = null!;

    public string? Line2 { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string? PostalCode { get; set; }

    public string? Phone { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Order> OrderBillingAddresses { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderShippingAddresses { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
