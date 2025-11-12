using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class StockTransaction
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Change { get; set; }

    public string? Reason { get; set; }

    public string? RelatedEntityId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
