using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Catalog
{
    public int IdProduct { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<PosOrder> PosOrders { get; set; } = new List<PosOrder>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
