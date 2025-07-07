using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using smelite_app.Models;

namespace smelite_app.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MasterProfile> MasterProfiles { get; set; }
        public DbSet<ApprenticeProfile> ApprenticeProfiles { get; set; }
        public DbSet<MasterProfileImage> MasterProfileImages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
