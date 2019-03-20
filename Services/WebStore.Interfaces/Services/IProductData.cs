using System.Collections.Generic;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Brand> GetBrands();

        Brand GetBrandById(int id);

        IEnumerable<Section> GetSections();

        Section GetSectionById(int id);

        PagedProductDTO GetProducts(ProductFilter Filter = null);

        ProductDTO GetProductById(int id);
    }
}
