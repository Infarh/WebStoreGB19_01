﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.InMemory
{
    class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;
        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            if (Filter is null) return TestData.Products;
            var result = TestData.Products.AsEnumerable();
            if (Filter.BrandId != null)
                result = result.Where(product => product.BrendId == Filter.BrandId);
            if(Filter.SectionId != null)
                result = result.Where(product => product.SectionId == Filter.SectionId);
            return result;
        }

        public Product GetProductById(int id)
        {
            return TestData.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}