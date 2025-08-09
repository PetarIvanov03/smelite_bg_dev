using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smelite_app.Controllers;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Helpers;
using Xunit;

namespace smelite_app.Tests.IntegrationTests
{
    public class ApprenticeControllerTests
    {
        [Fact]
        public async Task Order_PersistsApprenticeship()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            var apprenticeProfile = new ApprenticeProfile { Id = 1, ApplicationUserId = "u2" };
            var masterProfile = new MasterProfile { Id = 2, ApplicationUserId = "u1" };
            var offering = new CraftOffering { Id = 3, Craft = new Craft { MasterProfileCrafts = new List<MasterProfileCraft>{ new MasterProfileCraft{ MasterProfileId = 2 } } } };
            context.ApprenticeProfiles.Add(apprenticeProfile);
            context.MasterProfiles.Add(masterProfile);
            context.CraftOfferings.Add(offering);
            await context.SaveChangesAsync();

            var appRepo = new ApprenticeRepository(context);
            var craftRepo = new CraftRepository(context);
            var service = new ApprenticeService(appRepo, craftRepo, new Mock<IEmailSender>().Object);
            var paySvc = new Mock<IPaymentService>();
            paySvc.Setup(p => p.CreateCheckoutSessionAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("http://stripe");

            var user = new ApplicationUser { Id = "u2", Email = "e" };
            var userMgr = TestHelper.GetMockUserManager(user);
            var env = new Mock<IWebHostEnvironment>();

            var controller = new ApprenticeController(service, userMgr, env.Object, paySvc.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var result = await controller.Order(3);

            Assert.IsType<RedirectResult>(result);
            Assert.Equal(1, context.Apprenticeships.Count());
            var appr = context.Apprenticeships.Single();
            Assert.Equal(1, appr.ApprenticeProfileId);
            Assert.Equal(2, appr.MasterProfileId);
        }

        [Fact]
        public async Task Order_NoProfile_ReturnsNotFound()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            var user = new ApplicationUser { Id = "u3" };
            var userMgr = TestHelper.GetMockUserManager(user);
            var appRepo = new ApprenticeRepository(context);
            var craftRepo = new CraftRepository(context);
            var service = new ApprenticeService(appRepo, craftRepo, new Mock<IEmailSender>().Object);
            var env = new Mock<IWebHostEnvironment>();
            var paySvc = new Mock<IPaymentService>();
            paySvc.Setup(p => p.CreateCheckoutSessionAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("http://stripe");
            var controller = new ApprenticeController(service, userMgr, env.Object, paySvc.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var result = await controller.Order(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
