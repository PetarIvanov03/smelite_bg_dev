using Moq;
using smelite_app.Enums;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Helpers;
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
            craftRepo.Setup(r => r.GetCraftOfferingByIdAsync(1)).ReturnsAsync(new CraftOffering { Id = 1, Price = 50, Craft = new Craft { MasterProfileCrafts = new List<MasterProfileCraft>{ new MasterProfileCraft{ MasterProfileId=5 } } } });
            var emailSender = new Mock<IEmailSender>();
            var service = new ApprenticeService(appRepo.Object, craftRepo.Object, emailSender.Object);

            var pay = await service.AddApprenticeshipAsync(2, 1);
            Assert.NotNull(pay);

            appRepo.Verify(r => r.AddApprenticeshipAsync(It.Is<Apprenticeship>(a =>
                a.ApprenticeProfileId==2 && a.CraftOfferingId==1 && a.MasterProfileId==5 &&
                a.Status==ApprenticeshipStatus.Pending.ToString() && a.Payment!=null)), Times.Once);
            emailSender.Verify(s => s.SendEmailAsync(Variables.defaultEmail,
                EmailMessages.ApprenticeshipRequestedSubject,
                string.Format(EmailMessages.ApprenticeshipRequestedBody, 2,1)), Times.Once);
        }

        [Fact]
        public async Task AddApprenticeship_InvalidOffering_Throws()
        {
            var appRepo = new Mock<IApprenticeRepository>();
            var craftRepo = new Mock<ICraftRepository>();
            craftRepo.Setup(r => r.GetCraftOfferingByIdAsync(1)).ReturnsAsync((CraftOffering?)null);
            var service = new ApprenticeService(appRepo.Object, craftRepo.Object, new Mock<IEmailSender>().Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddApprenticeshipAsync(1,1));
        }
    }
}
