using Moq;
using smelite_app.Enums;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class ApprenticeServiceTests
    {
        [Fact]
        public async Task AddApprenticeship_Valid_AddsApprenticeship()
        {
            var appRepo = new Mock<IApprenticeRepository>();
            var craftRepo = new Mock<ICraftRepository>();
            craftRepo.Setup(r => r.GetCraftOfferingByIdAsync(1)).ReturnsAsync(new CraftOffering { Id = 1, Craft = new Craft { MasterProfileCrafts = new List<MasterProfileCraft>{ new MasterProfileCraft{ MasterProfileId=5 } } } });
            var service = new ApprenticeService(appRepo.Object, craftRepo.Object);

            await service.AddApprenticeshipAsync(2, 1);

            appRepo.Verify(r => r.AddApprenticeshipAsync(It.Is<Apprenticeship>(a => a.ApprenticeProfileId==2 && a.CraftOfferingId==1 && a.MasterProfileId==5 && a.Status==ApprenticeshipStatus.Pending.ToString())), Times.Once);
        }

        [Fact]
        public async Task AddApprenticeship_InvalidOffering_Throws()
        {
            var appRepo = new Mock<IApprenticeRepository>();
            var craftRepo = new Mock<ICraftRepository>();
            craftRepo.Setup(r => r.GetCraftOfferingByIdAsync(1)).ReturnsAsync((CraftOffering?)null);
            var service = new ApprenticeService(appRepo.Object, craftRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddApprenticeshipAsync(1,1));
        }
    }
}
