using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.TestApi;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPIControllerTests
    {
        [TestMethod]
        public void IndexReturnsViewWithValues()
        {
            var expectedValues = new[] { "1", "2", "3" };
            var valuesServiceMock = new Mock<IValueService>();
            valuesServiceMock.Setup(service => service.Get()).Returns(expectedValues);
            var loggerMock = new Mock<ILogger<WebAPIController>>();

            //  "Stab" mode
            var controller = new WebAPIController(valuesServiceMock.Object, loggerMock.Object);
            var viewResult = Assert.IsType<ViewResult>(controller.Index());
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);
            Assert.Equal(expectedValues.Length, model.Count());

            //  "Mock" mode
            valuesServiceMock.Verify(service => service.Get());
            valuesServiceMock.VerifyNoOtherCalls();
        }
    }
}