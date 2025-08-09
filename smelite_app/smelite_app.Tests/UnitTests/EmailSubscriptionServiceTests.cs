using Moq;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Helpers;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class EmailSubscriptionServiceTests
    {
        [Fact]
        public async Task Subscribe_NewEmail_AddsAndSends()
        {
            var repo = new Mock<IEmailSubscriptionRepository>();
            repo.Setup(r => r.GetByEmailAsync("t@test.com")).ReturnsAsync((EmailSubscription?)null);
            var emailSender = new Mock<IEmailSender>();
            var service = new EmailSubscriptionService(repo.Object, emailSender.Object);

            await service.SubscribeAsync("t@test.com");

            repo.Verify(r => r.AddAsync(It.Is<EmailSubscription>(s => s.Email == "t@test.com")), Times.Once);
            emailSender.Verify(s => s.SendEmailAsync("t@test.com", EmailMessages.SubscriptionConfirmSubject, EmailMessages.SubscriptionConfirmBody), Times.Once);
        }
    }
}
