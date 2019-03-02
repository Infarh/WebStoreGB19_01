using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class ProductsController : ControllerBase, IProductData
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData) => _ProductData = ProductData;

        #region Implementation of IProductData

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections()
        {
            return _ProductData.GetSections();
        }

        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()
        {
            return _ProductData.GetBrands();
        }

        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter Filter = null)
        {
            return _ProductData.GetProducts(Filter);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id)
        {
            return _ProductData.GetProductById(id);
        }

        #endregion
    }
}