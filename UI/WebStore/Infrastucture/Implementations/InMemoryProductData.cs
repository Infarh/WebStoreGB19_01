using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Infrastucture.Interfaces;

namespace WebStore.Infrastucture.Implementations
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
