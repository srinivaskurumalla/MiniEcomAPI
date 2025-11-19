using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class ProductTag
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Tag { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
