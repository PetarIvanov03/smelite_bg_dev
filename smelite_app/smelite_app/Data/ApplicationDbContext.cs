using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using smelite_app.Models;

namespace smelite_app.Data
{
    // Configure Identity to use the custom ApplicationUser and IdentityRole
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MasterProfile> MasterProfiles { get; set; }
        public DbSet<ApprenticeProfile> ApprenticeProfiles { get; set; }
        public DbSet<CraftImage> MasterProfileImages { get; set; }
        public DbSet<CraftImage> CraftImages { get; set; }
        public DbSet<Craft> Crafts { get; set; }
        public DbSet<CraftType> CraftTypes { get; set; }
        public DbSet<TrainingType> TrainingTypes { get; set; }
        public DbSet<MasterProfileCraft> MasterProfileCrafts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1:1 връзки за профилите
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.MasterProfile)
                .WithOne(mp => mp.ApplicationUser)
                .HasForeignKey<MasterProfile>(mp => mp.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.ApprenticeProfile)
                .WithOne(ap => ap.ApplicationUser)
                .HasForeignKey<ApprenticeProfile>(ap => ap.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MasterProfileCraft>()
                .HasKey(mpc => new { mpc.MasterProfileId, mpc.CraftId });
        }
    }
}
