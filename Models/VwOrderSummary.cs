using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class VwOrderSummary
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateTime OrderPlacedAt { get; set; }
}
