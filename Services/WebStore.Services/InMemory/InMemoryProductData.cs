﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;
using WebStore.Services.Map;

namespace WebStore.Services.InMemory
{
    internal class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;
        public Brand GetBrandById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Section> GetSections() => TestData.Sections;
        public Section GetSectionById(int id)
        {
            throw new System.NotImplementedException();
        }

        public PagedProductDTO GetProducts(ProductFilter Filter = null)
        {
            if (Filter is null) return new PagedProductDTO { Products = TestData.Products.Select(ProductDTO2Product.Map) };
            var result = TestData.Products.AsEnumerable();
            if (Filter.BrandId != null)
                result = result.Where(product => product.BrendId == Filter.BrandId);
            if (Filter.SectionId != null)
                result = result.Where(product => product.SectionId == Filter.SectionId);
            return new PagedProductDTO { Products = result.Select(ProductDTO2Product.Map) };
        }

        public ProductDTO GetProductById(int id)
        {
            return TestData.Products.FirstOrDefault(p => p.Id == id).Map();
        }
    }
}
