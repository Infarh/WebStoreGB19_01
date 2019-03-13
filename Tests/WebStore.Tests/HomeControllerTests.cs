using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.Api;


namespace WebStore.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _Controller;

        [TestInitialize]
        public void Initialize() => _Controller = new HomeController();

        [TestMethod]
        public void Index_Returns_View()
        {
            var result = _Controller.Index();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ValuesServiceTest_Method_Returns_With_Values()
        {
            var data = new [] { "1", "2", "3" };
            var expected_count = data.Length;

            var value_service_mock = new Mock<IValuesService>();
            value_service_mock
                .Setup(s => s.Get())
                .Returns(data);


            var result = _Controller.ValuesServiceTest(value_service_mock.Object);

            var view_result = Xunit.Assert.IsType<ViewResult>(result);

            var model = Xunit.Assert.IsAssignableFrom<IEnumerable<string>>(view_result.ViewData.Model);

            var actual_count = model.Count();
            Xunit.Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void ErrorStatus_404_Redirect_To_ErrorPage404()
        {
            var result = _Controller.ErrorStatus("404");

            var redirect_to_action_result = Xunit.Assert.IsType<RedirectToActionResult>(result);

            Xunit.Assert.Null(redirect_to_action_result.ControllerName);
            Xunit.Assert.Equal(nameof(HomeController.ErrorPage404), redirect_to_action_result.ActionName);
        }

        [TestMethod]
        public void ErrorStatus_Another_Returns_Contens_Result()
        {
            var error_id = "500";
            var expected_result = $"Статусный код ошибки {error_id}";

            var result = _Controller.ErrorStatus(error_id);

            var content_result = Xunit.Assert.IsType<ContentResult>(result);

            Xunit.Assert.Equal(expected_result, content_result.Content);
        }

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var result = _Controller.ContactUs();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Cart_Returns_View()
        {
            var result = _Controller.Cart();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var result = _Controller.BlogSingle();
            Xunit.Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Blog_Returns_View()
        {
            var result = _Controller.Blog();
            Xunit.Assert.IsType<ViewResult>(result);
        }
    }
}
