using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class PosOrder
{
    public int IdPos { get; set; }

    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Catalog IdProductNavigation { get; set; } = null!;
}
