using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using smelite_app.Controllers;
using smelite_app.Services;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Subscribe_InvalidEmail_RedirectsWithError()
        {
            var service = new Mock<IEmailSubscriptionService>();
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object, service.Object);
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            var result = await controller.Subscribe("");

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Невалиден имейл", controller.TempData["Notification"]);
            service.Verify(s => s.SubscribeAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Subscribe_ValidEmail_CallsServiceAndRedirects()
        {
            var service = new Mock<IEmailSubscriptionService>();
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object, service.Object);
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            var result = await controller.Subscribe("a@test.com");

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Благодарим за абонамента", controller.TempData["Notification"]);
            service.Verify(s => s.SubscribeAsync("a@test.com"), Times.Once);
        }
    }
}
