using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.ViewModels.Account;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class AccountServiceTests
    {
        private AccountService CreateService(out Mock<UserManager<ApplicationUser>> userMgrMock,
                                             out Mock<SignInManager<ApplicationUser>> signInMgrMock,
                                             out Mock<IMasterRepository> masterRepo,
                                             out Mock<IApprenticeRepository> apprenticeRepo)
        {
            userMgrMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            signInMgrMock = new Mock<SignInManager<ApplicationUser>>(userMgrMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
            masterRepo = new Mock<IMasterRepository>();
            apprenticeRepo = new Mock<IApprenticeRepository>();
            var logger = new Mock<ILogger<MasterRepository>>();
            return new AccountService(userMgrMock.Object, signInMgrMock.Object, masterRepo.Object, apprenticeRepo.Object, logger.Object);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsFailed()
        {
            var service = CreateService(out var userMgr, out var signInMgr, out _, out _);
            userMgr.Setup(m => m.FindByEmailAsync("email"))!.ReturnsAsync((ApplicationUser?)null);

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.False(result.Succeeded);
            signInMgr.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsSuccess()
        {
            var service = CreateService(out var userMgr, out var signInMgr, out _, out _);
            var user = new ApplicationUser { Email = "email" };
            userMgr.Setup(m => m.FindByEmailAsync("email"))!.ReturnsAsync(user);
            signInMgr.Setup(m => m.PasswordSignInAsync("email", "pwd", false, false)).ReturnsAsync(SignInResult.Success);

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task Login_InactiveApprentice_ReturnsNotAllowed()
        {
            var service = CreateService(out var userMgr, out var signInMgr, out _, out var appRepo);
            var user = new ApplicationUser { Id = "u1", Email = "email", Role = "Apprentice" };
            userMgr.Setup(m => m.FindByEmailAsync("email"))!.ReturnsAsync(user);
            appRepo.Setup(r => r.GetByUserIdAsync("u1"))!.ReturnsAsync(new ApprenticeProfile { IsActive = false });

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.False(result.Succeeded);
            signInMgr.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task Login_InactiveMaster_ReturnsNotAllowed()
        {
            var service = CreateService(out var userMgr, out var signInMgr, out var masterRepo, out _);
            var user = new ApplicationUser { Id = "u1", Email = "email", Role = "Master" };
            userMgr.Setup(m => m.FindByEmailAsync("email"))!.ReturnsAsync(user);
            masterRepo.Setup(r => r.GetByUserIdAsync("u1"))!.ReturnsAsync(new MasterProfile { IsActive = false });

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.False(result.Succeeded);
            signInMgr.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task Register_UserExists_ReturnsFailed()
        {
            var service = CreateService(out var userMgr, out _, out _, out _);
            userMgr.Setup(m => m.FindByEmailAsync("e"))!.ReturnsAsync(new ApplicationUser());

            var result = await service.RegisterAsync(new RegisterViewModel { Email = "e" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task Register_MasterRole_AddsMasterProfile()
        {
            var service = CreateService(out var userMgr, out _, out var masterRepo, out var appRepo);
            userMgr.Setup(m => m.FindByEmailAsync("e"))!.ReturnsAsync((ApplicationUser?)null);
            userMgr.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var vm = new RegisterViewModel { Email = "e", Password = "p", FirstName="f", LastName="l", Role="Master" };
            var result = await service.RegisterAsync(vm);

            Assert.True(result.Succeeded);
            masterRepo.Verify(m => m.AddProfileAsync(It.IsAny<MasterProfile>()), Times.Once);
            appRepo.Verify(m => m.AddProfileAsync(It.IsAny<ApprenticeProfile>()), Times.Never);
        }
    }
}
