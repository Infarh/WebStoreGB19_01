using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase, IProductData
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData productData) => _ProductData = productData;
        
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()
        {
            return _ProductData.GetBrands();
        }

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections()
        {
            return _ProductData.GetSections();
        }

        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDto> GetProducts([FromBody] ProductFilter Filter = null)
        {
            return _ProductData.GetProducts(Filter);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public ProductDto GetProductById(int id)
        {
            return _ProductData.GetProductById(id);
        }
    }
}