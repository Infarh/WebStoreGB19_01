using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void CartClass_ItemsCount_Returns_Correct_Quantity()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = 1,
                        Quantity = 1
                    },
                    new CartItem
                    {
                        ProductId = 2,
                        Quantity = 5
                    }
                }
            };
            var expectedCount = cart.Items.Sum(i => i.Quantity);

            var actualCount = cart.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            var cartViewModel = new CartViewModel
            {
                Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel { Id = 1, Name = "Item1", Price = 5m }, 1 },
                    { new ProductViewModel { Id = 2, Name = "Item2", Price = 10m }, 2 },
                }
            };

            var expectedCount = cartViewModel.Items.Sum(v => v.Value);

            var actualCount = cartViewModel.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartService_AddToCart_WorksCorrect()
        {
            const int expectedProductId = 5;

            var cart = new Cart { Items = new List<CartItem>() };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.AddToCart(expectedProductId);

            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(1, cart.ItemsCount);
            Assert.Equal(expectedProductId, cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_AddToCart_Increment_Quantity()
        {
            const int expectedProductId = 5;
            const int expectedItemsCount = 3;

            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = expectedProductId, Quantity = expectedItemsCount - 1 }
                }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.AddToCart(expectedProductId);

            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(expectedItemsCount, cart.ItemsCount);
        }

        [TestMethod]
        public void CartService_RemoveFromCart_Removes_Correct_Item()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 3 },
                    new CartItem { ProductId = 2, Quantity = 1 }
                }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.RemoveFromCart(1);

            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(2, cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartService_RemoveAll_Clear_Cart()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                 {
                     new CartItem { ProductId = 1, Quantity = 3 },
                     new CartItem { ProductId = 2, Quantity = 1 }
                 }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.RemoveAll();

            Assert.Equal(0, cart.Items.Count);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            var testItem = new CartItem { ProductId = 1, Quantity = 3 };
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    testItem,
                    new CartItem { ProductId = 2, Quantity = 1 }
                }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.DecrementFromCart(testItem.ProductId);

            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(3, cart.ItemsCount);
            Assert.Equal(2, testItem.Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement()
        {
            var testItem = new CartItem { ProductId = 2, Quantity = 1 };
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 3 },
                    testItem
                }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.DecrementFromCart(testItem.ProductId);

            Assert.Equal(3, cart.ItemsCount);
            Assert.Equal(1, cart.Items.Count);
            Assert.False(cart.Items.Contains(testItem));
        }

        [TestMethod]
        public void CartService_TransformCart_WorksCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 4 } }
            };

            var products = new List<ProductDto>
            {
                  new ProductDto
                  {
                      Id = 1,
                      ImageUrl = "image.jpg",
                      Name = "Test",
                      Order = 0,
                      Price = 1.11m,
                  }
            };
            
            var productDataMock = new Mock<IProductData>();
            productDataMock.Setup(c => c.GetProducts(It.IsAny<ProductFilter>())).Returns(products);

            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock.Setup(c => c.Cart).Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            var result = cartService.TransformCart();

            Assert.Equal(4, result.ItemsCount);
            Assert.Equal(1.11m, result.Items.First().Key.Price);
        }
    }
}
