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
            var expected_count = cart.Items.Sum(i => i.Quantity);

            var actual_count = cart.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartViewModel_Retulrns_Correct_ItemsCount()
        {
            var cart_view_model = new CartViewModel
            {
                Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel { Id = 1, Name = "Item1", Price = 5m }, 1 },
                    { new ProductViewModel { Id = 2, Name = "Item2", Price = 10m }, 2 },
                }
            };

            var expectes_count = cart_view_model.Items.Sum(v => v.Value);

            var actual_count = cart_view_model.ItemsCount;

            Assert.Equal(expectes_count, actual_count);
        }

        [TestMethod]
        public void CartService_AddToCart_WorksCorrect()
        {
            const int expected_product_id = 5;

            var cart = new Cart { Items = new List<CartItem>() };

            var product_data_mock = new Mock<IProductData>();
            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.AddToCart(expected_product_id);

            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(1, cart.ItemsCount);
            Assert.Equal(expected_product_id, cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_AddToCart_Increment_Quantity()
        {
            const int expected_product_id = 5;
            const int expected_items_count = 3;

            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = expected_product_id, Quantity = expected_items_count - 1 }
                }
            };

            var product_data_mock = new Mock<IProductData>();
            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.AddToCart(expected_product_id);

            Assert.Equal(1, cart.Items.Count);
            Assert.Equal(expected_items_count, cart.ItemsCount);
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

            var product_data_mock = new Mock<IProductData>();
            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.RemoveFromCart(1);

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

            var product_data_mock = new Mock<IProductData>();
            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.RemoveAll();

            Assert.Equal(0, cart.Items.Count);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            var test_item = new CartItem { ProductId = 1, Quantity = 3 };
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    test_item,
                    new CartItem { ProductId = 2, Quantity = 1 }
                }
            };

            var product_data_mock = new Mock<IProductData>();
            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.DecrementFromCart(test_item.ProductId);

            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(3, cart.ItemsCount);
            Assert.Equal(2, test_item.Quantity);

            //cart_service.DecrementFromCart(test_item.ProductId);
            //cart_service.DecrementFromCart(test_item.ProductId);

            //Assert.Equal(1, cart.Items.Count);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement()
        {
            var test_item = new CartItem { ProductId = 2, Quantity = 1 };
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 3 },
                    test_item
                }
            };

            var product_data_mock = new Mock<IProductData>();
            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.DecrementFromCart(test_item.ProductId);

            Assert.Equal(3, cart.ItemsCount);
            Assert.Equal(1, cart.Items.Count);
            Assert.False(cart.Items.Contains(test_item));
        }

        [TestMethod]
        public void CartService_TransformCart_WorksCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 4 } }
            };

            var products = new List<ProductDTO>
            {
                  new ProductDTO
                  {
                      Id = 1,
                      ImageUrl = "image.jpg",
                      Name = "Test",
                      Order = 0,
                      Price = 1.11m,
                  }
            };

            var product_data_mock = new Mock<IProductData>();
            product_data_mock.Setup(c => c.GetProducts(It.IsAny<ProductFilter>())).Returns(products);

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock.Setup(c => c.Cart).Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            var result = cart_service.TransformCart();

            Assert.Equal(4, result.ItemsCount);
            Assert.Equal(1.11m, result.Items.First().Key.Price);
        }
    }
}
