using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    namespace WebApplication1.Models
    {
        public class CartItems
        {
            public int CartItemId { get; set; }
            public int ProductId { get; set; }
            public int UserId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }

            public virtual Catalog Product { get; set; }
            public virtual User User { get; set; }
        }
    }
}
