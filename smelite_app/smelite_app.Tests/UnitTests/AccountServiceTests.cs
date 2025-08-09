using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Moq;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Helpers;
using smelite_app.ViewModels.Account;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class AccountServiceTests
    {
        private AccountService CreateService(out Mock<IAccountRepository> accountRepo,
                                             out Mock<IMasterRepository> masterRepo,
                                             out Mock<IApprenticeRepository> apprenticeRepo)
        {
            accountRepo = new Mock<IAccountRepository>();
            masterRepo = new Mock<IMasterRepository>();
            apprenticeRepo = new Mock<IApprenticeRepository>();
            var logger = new Mock<ILogger<AccountService>>();
            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var emailSender = new Mock<IEmailSender>();
            return new AccountService(accountRepo.Object,
                                     masterRepo.Object,
                                     apprenticeRepo.Object,
                                     logger.Object,
                                     urlHelperFactory.Object,
                                     httpContextAccessor.Object,
                                     emailSender.Object);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsFailed()
        {
            var service = CreateService(out var accountRepo, out _, out _);
            accountRepo.Setup(r => r.FindByEmailAsync("email"))!.ReturnsAsync((ApplicationUser?)null);

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.False(result.Succeeded);
            accountRepo.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsSuccess()
        {
            var service = CreateService(out var accountRepo, out _, out _);
            var user = new ApplicationUser { Email = "email" };
            accountRepo.Setup(r => r.FindByEmailAsync("email"))!.ReturnsAsync(user);
            accountRepo.Setup(r => r.PasswordSignInAsync("email", "pwd", false, false)).ReturnsAsync(SignInResult.Success);

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task Login_InactiveApprentice_ReturnsNotAllowed()
        {
            var service = CreateService(out var accountRepo, out _, out var appRepo);
            var user = new ApplicationUser { Id = "u1", Email = "email", Role = "Apprentice" };
            accountRepo.Setup(r => r.FindByEmailAsync("email"))!.ReturnsAsync(user);
            appRepo.Setup(r => r.GetByUserIdAsync("u1"))!.ReturnsAsync(new ApprenticeProfile { IsActive = false });

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.False(result.Succeeded);
            accountRepo.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task Login_InactiveMaster_ReturnsNotAllowed()
        {
            var service = CreateService(out var accountRepo, out var masterRepo, out _);
            var user = new ApplicationUser { Id = "u1", Email = "email", Role = "Master" };
            accountRepo.Setup(r => r.FindByEmailAsync("email"))!.ReturnsAsync(user);
            masterRepo.Setup(r => r.GetByUserIdAsync("u1"))!.ReturnsAsync(new MasterProfile { IsActive = false });

            var result = await service.LoginAsync(new LoginViewModel { Email = "email", Password = "pwd" });

            Assert.False(result.Succeeded);
            accountRepo.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
        }

        [Fact]
        public async Task Register_UserExists_ReturnsFailed()
        {
            var service = CreateService(out var accountRepo, out _, out _);
            accountRepo.Setup(r => r.FindByEmailAsync("e"))!.ReturnsAsync(new ApplicationUser());

            var result = await service.RegisterAsync(new RegisterViewModel { Email = "e" });

            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task Register_MasterRole_AddsMasterProfile()
        {
            var service = CreateService(out var accountRepo, out var masterRepo, out var appRepo);
            accountRepo.Setup(r => r.FindByEmailAsync("e"))!.ReturnsAsync((ApplicationUser?)null);
            accountRepo.Setup(r => r.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var vm = new RegisterViewModel { Email = "e", Password = "p", FirstName="f", LastName="l", Role="Master" };
            var result = await service.RegisterAsync(vm);

            Assert.True(result.Succeeded);
            masterRepo.Verify(m => m.AddProfileAsync(It.IsAny<MasterProfile>()), Times.Once);
            appRepo.Verify(m => m.AddProfileAsync(It.IsAny<ApprenticeProfile>()), Times.Never);
        }
    }
}
