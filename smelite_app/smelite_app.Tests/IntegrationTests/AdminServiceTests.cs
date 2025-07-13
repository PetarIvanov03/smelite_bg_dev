using System;
using smelite_app.Models;
using smelite_app.Enums;
using smelite_app.Services;
using Xunit;

namespace smelite_app.Tests.IntegrationTests
{
    public class AdminServiceTests
    {
        [Fact]
        public async Task ToggleUserActivation_UpdatesProfile()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            var user = new ApplicationUser { Id = "u1", Email = "e" };
            var profile = new ApprenticeProfile { Id = 1, ApplicationUserId = "u1", ApplicationUser = user, IsActive = true };
            context.Users.Add(user);
            context.ApprenticeProfiles.Add(profile);
            await context.SaveChangesAsync();

            var service = new AdminService(context);
            await service.ToggleUserActivationAsync("u1", false);

            Assert.False(context.ApprenticeProfiles.Single().IsActive);
        }

        [Fact]
        public async Task UpdateApprenticeshipStatus_SavesStatus()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            var app = new Apprenticeship { Id = 1, ApprenticeProfileId = 1, MasterProfileId = 1, CraftOfferingId = 1, Status = ApprenticeshipStatus.Pending.ToString() };
            context.Apprenticeships.Add(app);
            await context.SaveChangesAsync();

            var service = new AdminService(context);
            await service.UpdateApprenticeshipStatusAsync(1, ApprenticeshipStatus.Active.ToString());

            Assert.Equal(ApprenticeshipStatus.Active.ToString(), context.Apprenticeships.Single().Status);
        }

        [Fact]
        public async Task UpdatePaymentStatus_Success_ActivatesApprenticeship()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            var app = new Apprenticeship { Id = 1, ApprenticeProfileId = 1, MasterProfileId = 1, CraftOfferingId = 1, Status = ApprenticeshipStatus.Pending.ToString() };
            context.Apprenticeships.Add(app);
            await context.SaveChangesAsync();
            var pay = new Payment { Id = 1, ApprenticeshipId = 1, PayerProfileId = 1, RecipientProfileId = 1, AmountTotal = 10, PlatformFee = 1, AmountToRecipient = 9, PaidOn = DateTime.UtcNow, Method = "Cash", Status = PaymentStatus.Pending.ToString(), TransactionId = "t" };
            context.Payments.Add(pay);
            await context.SaveChangesAsync();

            var service = new AdminService(context);
            await service.UpdatePaymentStatusAsync(1, PaymentStatus.Success.ToString());

            Assert.Equal(PaymentStatus.Success.ToString(), context.Payments.Single().Status);
            Assert.Equal(ApprenticeshipStatus.Active.ToString(), context.Apprenticeships.Single().Status);
        }
    }
}
