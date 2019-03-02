using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Services.Sql
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands.AsEnumerable();
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections.AsEnumerable();
        }

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            if (Filter is null)
                return _db.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Section)
                    .AsEnumerable()
                    .Select(ProductDTO2Product.Map);

            IQueryable<Product> result = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter.BrandId != null)
                result = result.Where(p => p.BrendId == Filter.BrandId);
            if (Filter.SectionId != null)
                result = result.Where(p => p.SectionId == Filter.SectionId);
            return result.AsEnumerable().Select(ProductDTO2Product.Map);
        }

        public ProductDTO GetProductById(int id)
        {
            return _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .FirstOrDefault(p => p.Id == id)
                .Map();
        }
    }
}
