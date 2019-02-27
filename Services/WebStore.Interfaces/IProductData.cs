using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Brand> GetBrands();
        IEnumerable<Section> GetSections();

        IEnumerable<Product> GetProducts(ProductFilter Filter = null);
        Product GetProductById(int id);
    }
}
