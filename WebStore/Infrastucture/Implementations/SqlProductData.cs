using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastucture.Interfaces;

namespace WebStore.Infrastucture.Implementations
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

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            if (Filter is null)
                return _db.Products.AsEnumerable();
            IQueryable<Product> result = _db.Products;
            if (Filter.BrandId != null)
                result = result.Where(p => p.BrendId == Filter.BrandId);
            if (Filter.SectionId != null)
                result = result.Where(p => p.SectionId == Filter.SectionId);
            return result.AsEnumerable();
        }

        public Product GetProductById(int id)
        {
            return _db.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
