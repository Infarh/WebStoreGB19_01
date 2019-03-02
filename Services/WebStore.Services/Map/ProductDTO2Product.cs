using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class ProductDTO2Product
    {
        public static ProductDTO Map(this Product product) =>
            new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand is null
                    ? null
                    : new BrandDTO
                    {
                        Id = product.Brand.Id,
                        Name = product.Brand.Name
                    }
            };
    }
}
