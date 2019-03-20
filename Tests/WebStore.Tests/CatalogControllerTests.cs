using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Xunit.Sdk;

using Assert = Xunit.Assert;

namespace WebStore.Tests
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_View_With_Correct_Item()
        {
            const int expected_id = 15;
            const int expected_brand_id = 10;

            const string expected_name = "Product name";
            const int expected_order = 1;
            const decimal excpected_price = 11;
            const string expected_image_url = "image.jpg";
            const string expected_brand_name = "Brand name";

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new ProductDTO
                {
                    Id = id,
                    Name = expected_name,
                    Order = expected_order,
                    Price = excpected_price,
                    ImageUrl = expected_image_url,
                    Brand = new BrandDTO
                    {
                        Id = expected_brand_id,
                        Name = expected_brand_name
                    }
                });
            var configuration_mock = new Mock<IConfiguration>();

            var catalog_controller = new CatalogController(product_data_mock.Object, configuration_mock.Object);

            var result = catalog_controller.ProductDetails(expected_id);

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.ViewData.Model);

            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_order, model.Order);
            Assert.Equal(excpected_price, model.Price);
            Assert.Equal(expected_image_url, model.ImageUrl);
            Assert.Equal(expected_brand_name, model.Brand);
        }

        [TestMethod]
        public void ProductDetails_Product_Not_Found()
        {
            var product_data_mock = new Mock<IProductData>();
            product_data_mock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns((ProductDTO)null);
            var configuration_mock = new Mock<IConfiguration>();

            var catalog_controller = new CatalogController(product_data_mock.Object, configuration_mock.Object);

            var result = catalog_controller.ProductDetails(-1);

            var not_fount_result = Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Method_Returns_Correct_View()
        {
            const int expected_brand_id = 5;
            const int expected_section_id = 10;

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter => new PagedProductDTO
                {
                    Products = new[]
                    {
                        new ProductDTO
                        {
                            Id = 1,
                            Name = "Product 1",
                            Order = 1,
                            Price = 10,
                            ImageUrl = "Image1.jpg",
                            Brand = new BrandDTO
                            {
                                Id = 1,
                                Name = "Brand 1"
                            }
                        },
                        new ProductDTO
                        {
                            Id = 2,
                            Name = "Product 2",
                            Order = 2,
                            Price = 20,
                            ImageUrl = "Image2.jpg",
                            Brand = new BrandDTO
                            {
                                Id = 1,
                                Name = "Brand 1"
                            }
                        }
                    }
                });
            var configuration_mock = new Mock<IConfiguration>();


            var catalog_controller = new CatalogController(product_data_mock.Object, configuration_mock.Object);

            var result = catalog_controller.Shop(expected_section_id, expected_brand_id);

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_section_id, model.SectionId);
        }
    }
}
