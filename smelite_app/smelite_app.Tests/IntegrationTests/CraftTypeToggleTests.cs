using Microsoft.EntityFrameworkCore;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Models;
using Xunit;

namespace smelite_app.Tests.IntegrationTests
{
    public class CraftTypeToggleTests
    {
        [Fact]
        public async Task ToggleCraftType_SetsIsDeletedFlag()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            var type = new CraftType { Id = 1, Name = "Type", IsDeleted = false };
            context.CraftTypes.Add(type);
            await context.SaveChangesAsync();

            var repo = new CraftRepository(context);
            var service = new CraftService(repo);

            await service.ToggleCraftTypeAsync(1, false);
            var stored = context.CraftTypes.IgnoreQueryFilters().Single(ct => ct.Id == 1);
            Assert.True(stored.IsDeleted);

            await service.ToggleCraftTypeAsync(1, true);
            stored = context.CraftTypes.IgnoreQueryFilters().Single(ct => ct.Id == 1);
            Assert.False(stored.IsDeleted);
        }
    }
}
