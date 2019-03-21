using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CheckOut_ModelState_Invalid_Returns_ViewModel()
        {
            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();
            var loggerMock = new Mock<ILogger<CartController>>();

            var controller = new CartController(cartServiceMock.Object, orderServiceMock.Object);

            controller.ModelState.AddModelError("error", "InvalidModel");

            const string expectedName = "Model Name";
            var result = controller.CheckOut(new OrderViewModel {Name = expectedName}, loggerMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.ViewData.Model);

            Assert.Equal(expectedName, model.OrderViewModel.Name);
        }

        [TestMethod]
        public void CheckOut_Calls_Service_And_Return_Redirect()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUserName")
            }));

            var cartServiceMock = new Mock<ICartService>();

            cartServiceMock
                .Setup(c => c.TransformCart())
                .Returns(new CartViewModel
                {
                    Items = new Dictionary<ProductViewModel, int>
                    {
                        {
                            new ProductViewModel(),
                            1
                        }
                    }
                });

            var ordersServiceMock = new Mock<IOrderService>();

            ordersServiceMock
                .Setup(o => o.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>()))
                .Returns<CreateOrderModel, string>((model, userName) => new OrderDto() {Id = 1});

            var loggerMock = new Mock<ILogger<CartController>>();

            var controller = new CartController(cartServiceMock.Object, ordersServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext {User = user}
                }
            };

            var orderViewModel = new OrderViewModel
            {
                Name = "test",
                Address = "address",
                PhoneNumber = "+7(123)456-78-90"
            };

            var result = controller.CheckOut(orderViewModel, loggerMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.Equal(1, redirectResult.RouteValues["id"]);
        }
    }
}