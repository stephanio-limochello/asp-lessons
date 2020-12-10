using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using WebStore.Controllers;
using Assert = Xunit.Assert;
using System;

namespace WebStore.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			object obj = null;
			Assert.Null(obj);
		}

		[TestMethod]
		public void IndexReturnsView()
		{
			var controller = new HomeController();
			var result = controller.Index();
			Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogsReturnsView()
        {
            var controller = new HomeController();
            var result = controller.Blogs();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogSingleReturnsView()
        {
            var controller = new HomeController();
            var result = controller.BlogSingle();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUsReturnsView()
        {
            var controller = new HomeController();
            var result = controller.ContactUs();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Error404ReturnsView()
        {
            var controller = new HomeController();
            var result = controller.Error404();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void ThrowApplicationException()
        {
            var controller = new HomeController();
            const string exceptionMessage = "";
            controller.Throw(exceptionMessage);
        }

        [TestMethod]
        public void ThrowApplicationException2()
        {
            var controller = new HomeController();
            Exception error = null;
            const string exception_message = "";
            try
            {
                controller.Throw(exception_message);
            }
            catch (Exception e)
            {
                error = e;
            }
            var applicationException = Assert.IsType<ApplicationException>(error);
            Assert.Equal($"Exception: {exception_message}", applicationException.Message);
        }

        [TestMethod]
        public void ThrowApplicationException3()
        {
            var controller = new HomeController();
            const string exception_message = "";
            var error = Assert.Throws<ApplicationException>(() => controller.Throw(exception_message));
            Assert.Equal($"Exception: {exception_message}", error.Message);
        }

        [TestMethod]
        public void ErrorStatus404RedirectToError404()
        {
            var controller = new HomeController();
            const string statusCode404 = "404";
            const string expectedMethodName = nameof(HomeController.Error404);
            var result = controller.ErrorStatus(statusCode404);
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(expectedMethodName, redirectToAction.ActionName);
            Assert.Null(redirectToAction.ControllerName);
        }
    }
}
