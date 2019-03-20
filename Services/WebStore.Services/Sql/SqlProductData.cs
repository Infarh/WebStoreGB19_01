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

        public SqlProductData(WebStoreContext db) => _db = db;

        public IEnumerable<Brand> GetBrands() => _db.Brands.AsEnumerable();

        public Brand GetBrandById(int id) => _db.Brands.FirstOrDefault(b => b.Id == id);

        public IEnumerable<Section> GetSections() => _db.Sections.AsEnumerable();

        public Section GetSectionById(int id) => _db.Sections.FirstOrDefault(s => s.Id == id);

        public PagedProductDTO GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);
            if (Filter is null)
                return new PagedProductDTO
                {
                    Products = query.AsEnumerable().Select(ProductDTO2Product.Map),
                    TotalCount = query.Count()
                };

            IQueryable<Product> result = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter.BrandId != null)
                result = result.Where(p => p.BrendId == Filter.BrandId);
            if (Filter.SectionId != null)
                result = result.Where(p => p.SectionId == Filter.SectionId);

            var total_count = result.Count();

            if (Filter.PageSize != null)
                result = result
                    .Skip((Filter.Page - 1) * (int) Filter.PageSize)
                    .Take((int)Filter.PageSize);

            return new PagedProductDTO
            {
                Products = result.AsEnumerable().Select(ProductDTO2Product.Map),
                TotalCount = total_count
            };
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
