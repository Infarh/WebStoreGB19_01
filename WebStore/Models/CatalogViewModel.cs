using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class CatalogViewModel
    {
        public int? BrandId { get; set; }
        public int? SectionId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }

    public class ProductViewModel
    {
         public int Id { get; set; }
         public string Name { get; set; }
         public string Brand { get; set; }
         public int Order { get; set; }
         public decimal Price { get; set; }
         public string ImageUrl { get; set; }
    }
}
