using System.Collections.Generic;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Brand> GetBrands();
        
        IEnumerable<Section> GetSections();

        IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null);
        
        ProductDto GetProductById(int id);
    }
}
