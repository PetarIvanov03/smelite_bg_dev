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
        public DbSet<CraftImage> CraftImages { get; set; }
        public DbSet<Craft> Crafts { get; set; }
        public DbSet<CraftType> CraftTypes { get; set; }
        public DbSet<MasterProfileCraft> MasterProfileCrafts { get; set; }
        public DbSet<Apprenticeship> Apprenticeships { get; set; }
        public DbSet<CraftLocation> CraftLocations { get; set; }
        public DbSet<CraftPackage> CraftPackages { get; set; }
        public DbSet<CraftOffering> CraftOfferings { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.MasterProfile)
                .WithOne(mp => mp.ApplicationUser)
                .HasForeignKey<MasterProfile>(mp => mp.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.ApprenticeProfile)
                .WithOne(ap => ap.ApplicationUser)
                .HasForeignKey<ApprenticeProfile>(ap => ap.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MasterProfileCraft>()
                .HasKey(mpc => new { mpc.MasterProfileId, mpc.CraftId });

            builder.Entity<Apprenticeship>()
                .HasOne(a => a.MasterProfile)
                .WithMany(mp => mp.Tasks)
                .HasForeignKey(a => a.MasterProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Apprenticeship>()
                .HasOne(a => a.ApprenticeProfile)
                .WithMany(ap => ap.Apprenticeships)
                .HasForeignKey(a => a.ApprenticeProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Apprenticeship>()
                .HasOne(a => a.Payment)
                .WithOne(o => o.Apprenticeship)
                .HasForeignKey<Payment>(o => o.ApprenticeshipId)
                .IsRequired();

            builder.Entity<Payment>()
                .HasOne(p => p.PayerProfile)
                .WithMany(ap => ap.Payments)
                .HasForeignKey(p => p.PayerProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasOne(p => p.RecipientProfile)
                .WithMany()
                .HasForeignKey(p => p.RecipientProfileId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
