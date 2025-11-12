using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public decimal Price { get; set; }

    public decimal? Mrp { get; set; }

    public decimal TaxPercent { get; set; }

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
