﻿using System.Collections.Generic;

namespace WebStore.Domain.ViewModels
{
    public class CatalogViewModel
    {
        public int? BrandId { get; set; }
        public int? SectionId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
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
