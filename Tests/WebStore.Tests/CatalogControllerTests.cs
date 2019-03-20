using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_View_With_Correct_Item()
        {
            const int expectedId = 15;
            const int expectedBrandId = 10;

            const string expectedName = "Product name";
            const int expectedOrder = 1;
            const decimal expectedPrice = 11;
            const string expectedImageUrl = "image.jpg";
            const string expectedBrandName = "Brand name";

            var productDataMock = new Mock<IProductData>();
            
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new ProductDto()
                {
                    Id = id,
                    Name = expectedName,
                    Order = expectedOrder,
                    Price = expectedPrice,
                    ImageUrl = expectedImageUrl,
                    Brand = new BrandDto()
                    {
                        Id = expectedBrandId,
                        Name = expectedBrandName
                    }
                });

            var catalogController = new CatalogController(productDataMock.Object);

            var result = catalogController.ProductDetails(expectedId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);

            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedOrder, model.Order);
            Assert.Equal(expectedPrice, model.Price);
            Assert.Equal(expectedImageUrl, model.ImageUrl);
            Assert.Equal(expectedBrandName, model.Brand);
        }

        [TestMethod]
        public void ProductDetails_Product_Not_Found()
        {
            var productDataMock = new Mock<IProductData>();
            
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns((ProductDto) null);

            var catalogController = new CatalogController(productDataMock.Object);

            var result = catalogController.ProductDetails(-1);

            var notFountResult = Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Method_Returns_Correct_View()
        {
            const int expectedBrandId = 5;
            const int expectedSectionId = 10;

            var productDataMock = new Mock<IProductData>();
            
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter => new[]
                {
                    new ProductDto()
                    {
                        Id = 1,
                        Name = "Product 1",
                        Order = 1,
                        Price = 10,
                        ImageUrl = "Image1.jpg",
                        Brand = new BrandDto()
                        {
                            Id = 1,
                            Name = "Brand 1"
                        }
                    },
                    new ProductDto()
                    {
                        Id = 2,
                        Name = "Product 2",
                        Order = 2,
                        Price = 20,
                        ImageUrl = "Image2.jpg",
                        Brand = new BrandDto()
                        {
                            Id = 1,
                            Name = "Brand 1"
                        }
                    }
                });

            var catalogController = new CatalogController(productDataMock.Object);

            var result = catalogController.Shop(expectedSectionId, expectedBrandId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<CatalogViewModel>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expectedBrandId, model.BrandId);
            Assert.Equal(expectedSectionId, model.SectionId);
        }
    }
}