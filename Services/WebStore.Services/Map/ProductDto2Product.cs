using WebStore.DAL.Migrations;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class ProductDto2Product
    {
        public static ProductDto Map(this Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand is null
                ? null
                : new BrandDto()
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                }
            };
        }
    }
}