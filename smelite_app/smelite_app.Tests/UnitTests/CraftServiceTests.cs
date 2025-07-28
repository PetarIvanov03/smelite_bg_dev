using Moq;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Models;
using Xunit;

namespace smelite_app.Tests.UnitTests
{
    public class CraftServiceTests
    {
        [Fact]
        public async Task SoftDeleteCraft_CallsRepository()
        {
            var repo = new Mock<ICraftRepository>();
            var service = new CraftService(repo.Object);

            await service.SoftDeleteCraftAsync(5);

            repo.Verify(r => r.SoftDeleteCraftAsync(5), Times.Once);
        }

        [Fact]
        public async Task GetCraftById_ReturnsFromRepository()
        {
            var repo = new Mock<ICraftRepository>();
            var craft = new Craft { Id = 3 };
            repo.Setup(r => r.GetCraftByIdAsync(3)).ReturnsAsync(craft);
            var service = new CraftService(repo.Object);

            var result = await service.GetCraftByIdAsync(3);

            Assert.Equal(craft, result);
        }

        [Fact]
        public async Task ToggleCraftType_CallsRepository()
        {
            var repo = new Mock<ICraftRepository>();
            var service = new CraftService(repo.Object);

            await service.ToggleCraftTypeAsync(2, true);

            repo.Verify(r => r.ToggleCraftTypeAsync(2, true), Times.Once);
        }
    }
}
