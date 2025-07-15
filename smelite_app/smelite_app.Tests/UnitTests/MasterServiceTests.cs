using Microsoft.AspNetCore.Hosting;
using Moq;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.ViewModels.Master;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class MasterServiceTests
    {
        [Fact]
        public async Task GetProfile_ReturnsViewModel()
        {
            var repo = new Mock<IMasterRepository>();
            var profile = new MasterProfile
            {
                ApplicationUser = new ApplicationUser { FirstName = "f", LastName = "l", Email = "e", ProfileImageUrl = "p" },
                PersonalInformation = "pi"
            };
            repo.Setup(r => r.GetByUserIdAsync("1")).ReturnsAsync(profile);
            var service = new MasterService(repo.Object, new EmailSender());

            var result = await service.GetProfileAsync("1");

            Assert.NotNull(result);
            Assert.Equal("f", result!.FirstName);
        }

        [Fact]
        public async Task AddCraft_CallsRepository()
        {
            var repo = new Mock<IMasterRepository>();
            var env = new Mock<IWebHostEnvironment>();
            var service = new MasterService(repo.Object, new EmailSender());
            var vm = new CraftViewModel { Name = "n", CraftDescription="d", ExperienceYears=1, CraftTypeId=1, Offerings=new List<CraftOfferingFormViewModel>() };

            await service.AddCraftAsync(2, vm, "root", "user");

            repo.Verify(r => r.AddCraftAsync(2, It.IsAny<Craft>(), It.IsAny<IEnumerable<CraftOffering>>(), It.IsAny<IEnumerable<CraftImage>>()), Times.Once);
        }
    }
}
