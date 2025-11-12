using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
