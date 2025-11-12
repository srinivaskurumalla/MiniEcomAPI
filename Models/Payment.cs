using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? PaymentProvider { get; set; }

    public string? PaymentReference { get; set; }

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
