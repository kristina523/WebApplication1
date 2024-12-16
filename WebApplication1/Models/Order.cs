using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int IdUser { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalSum { get; set; }

    public string? Status { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<PosOrder> PosOrders { get; set; } = new List<PosOrder>();
}
