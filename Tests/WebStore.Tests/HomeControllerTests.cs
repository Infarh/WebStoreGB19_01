using System.Collections.Generic;
using System.Linq;
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
        public void Initialize()
        {
            _Controller = new HomeController();
        }

        [TestMethod]
        public void Index_Returns_View()
        {
            var result = _Controller.Index();

            Xunit.Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ValuesServiceTest_Method_Returns_With_Values()
        {
            var data = new[] {"1", "2"};
            var expectedCunt = data.Length;
            
            var valueServiceMock = new Mock<IValuesService>();
            valueServiceMock
                .Setup(s => s.Get())
                .Returns(data); 
            
            var result = _Controller.ValuesServiceTest(valueServiceMock.Object);
            
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);

            var model = Xunit.Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.ViewData.Model);

            var actualCount = model.Count();
            Xunit.Assert.Equal(expectedCunt, data.Length);
        }

        [TestMethod]
        public void ErrorStatus_404_Redirect_To_ErrorPage404()
        {
            var result = _Controller.ErrorStatus("404");

            var redirectToActionResult = Xunit.Assert.IsType<RedirectToActionResult>(result);
            
            Xunit.Assert.Null(redirectToActionResult.ControllerName);
            Xunit.Assert.Equal(nameof(HomeController.ErrorPage404), redirectToActionResult.ActionName);
        }
        
        [TestMethod]
        public void ErrorStatus_Another_Returns_Contents_Results()
        {
            const string errorId = "500";
            var expectedResult = $"Error status {errorId}";
            
            var result = _Controller.ErrorStatus(errorId);

            var contentResult = Xunit.Assert.IsType<ContentResult>(result);

            Xunit.Assert.Equal(expectedResult, contentResult.Content);
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