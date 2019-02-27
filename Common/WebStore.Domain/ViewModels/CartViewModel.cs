using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.ViewModels
{
    public class CartViewModel
    {
        public Dictionary<ProductViewModel, int> Items { get; set; } = new Dictionary<ProductViewModel, int>();
        public int ItemsCount => Items?.Sum(i => i.Value) ?? 0;
    }

    public class Cart
    {
         public List<CartItem> Items { get; set; } = new List<CartItem>();

         public int ItemsCount => Items?.Sum(i => i.Quantity) ?? 0;

    }

    public class CartItem
    {
         public int ProductId { get; set; }
         public int Quantity { get; set; }
    }
}
