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

        // Тук добавяш и други ентитита (Product, Order, Apprenticeship и т.н.)

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API за 1:1 връзки (пример):
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

            // По избор – Fluent API за други ентитита
        }
    }
}
