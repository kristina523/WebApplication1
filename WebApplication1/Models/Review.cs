using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Review
{
    public int IdReview { get; set; }

    public int IdUser { get; set; }

    public int IdProduct { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? ReviewDate { get; set; }

    public virtual Catalog IdProductNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
