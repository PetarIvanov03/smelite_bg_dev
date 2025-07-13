using Microsoft.EntityFrameworkCore;
using smelite_app.Repositories;
using smelite_app.Services;
using smelite_app.Models;
using Xunit;

namespace smelite_app.Tests.IntegrationTests
{
    public class CraftSoftDeleteTests
    {
        [Fact]
        public async Task SoftDeleteCraft_SetsFlagAndExcludedFromQueries()
        {
            using var context = TestHelper.GetInMemoryDbContext();
            context.CraftTypes.Add(new CraftType { Id = 1, Name = "Type" });
            var craft = new Craft { Id = 1, Name = "Craft", CraftTypeId=1 };
            context.Crafts.Add(craft);
            await context.SaveChangesAsync();

            var repo = new CraftRepository(context);
            var service = new CraftService(repo);

            await service.SoftDeleteCraftAsync(1);

            var stored = context.Crafts.IgnoreQueryFilters().Single(c => c.Id==1);
            Assert.True(stored.IsDeleted);
            Assert.Null(await service.GetCraftByIdAsync(1));
        }
    }
}
