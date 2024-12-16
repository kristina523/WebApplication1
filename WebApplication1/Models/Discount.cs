using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Discount
{
    public int IdDiscount { get; set; }

    public int IdProduct { get; set; }

    public decimal? DiscountPercent { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Catalog IdProductNavigation { get; set; } = null!;
}
