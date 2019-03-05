using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;
using WebStore.Services.Map;

namespace WebStore.Services.InMemory
{
    class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;
        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null)
        {
            if (Filter is null) return TestData.Products.Select(ProductDto2Product.Map);
            
            var result = TestData.Products.AsEnumerable();
            
            if (Filter.BrandId != null)
                result = result.Where(product => product.BrandId == Filter.BrandId);
            
            if(Filter.SectionId != null)
                result = result.Where(product => product.SectionId == Filter.SectionId);

            return result.Select(ProductDto2Product.Map);
        }

        public ProductDto GetProductById(int id)
        {
            return TestData.Products.FirstOrDefault(p => p.Id == id).Map();
        }
    }
}
