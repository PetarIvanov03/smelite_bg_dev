using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using smelite_app.Controllers;
using smelite_app.Data;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.ViewModels.Master;
using Xunit;

namespace smelite_app.Tests.IntegrationTests
{
    public class MasterControllerTests
    {
        [Fact]
        public async Task CreateCraft_PersistsCraft()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            context.CraftTypes.Add(new CraftType { Id = 1, Name = "Type" });
            var masterProfile = new MasterProfile { Id = 1, ApplicationUserId = "u1" };
            context.MasterProfiles.Add(masterProfile);
            await context.SaveChangesAsync();

            var masterRepo = new MasterRepository(context);
            var craftRepo = new CraftRepository(context);
            var masterService = new MasterService(masterRepo);
            var craftService = new CraftService(craftRepo);

            var user = new ApplicationUser { Id = "u1", Email = "e" };
            var userMgr = TestHelper.GetMockUserManager(user);

            var env = new Mock<IWebHostEnvironment>();
            env.SetupGet(e => e.WebRootPath).Returns("/tmp");

            var controller = new MasterController(masterService, userMgr, craftService, env.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            var vm = new CraftViewModel
            {
                Name = "Craft",
                CraftDescription = "desc",
                ExperienceYears = 1,
                CraftTypeId = 1,
                Offerings = new List<CraftOfferingFormViewModel>()
            };

            var result = await controller.CreateCraft(vm);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(1, context.Crafts.Count());
            Assert.Equal("Craft", context.Crafts.Single().Name);
        }

        [Fact]
        public async Task CreateCraft_NoProfile_ReturnsNotFound()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            context.CraftTypes.Add(new CraftType { Id = 1, Name = "Type" });
            await context.SaveChangesAsync();

            var masterRepo = new MasterRepository(context);
            var craftRepo = new CraftRepository(context);
            var masterService = new MasterService(masterRepo);
            var craftService = new CraftService(craftRepo);

            var user = new ApplicationUser { Id = "u1", Email = "e" };
            var userMgr = TestHelper.GetMockUserManager(user);
            var env = new Mock<IWebHostEnvironment>();
            env.SetupGet(e => e.WebRootPath).Returns("/tmp");

            var controller = new MasterController(masterService, userMgr, craftService, env.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            var vm = new CraftViewModel { Name = "Craft", CraftDescription="d", ExperienceYears=1, CraftTypeId=1, Offerings=new List<CraftOfferingFormViewModel>() };

            var result = await controller.CreateCraft(vm);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
