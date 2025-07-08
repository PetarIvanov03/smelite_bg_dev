using Microsoft.AspNetCore.Identity;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Seed
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            string[] roleNames = { "Admin", "Master", "Apprentice" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            //Admin account

            string adminEmail = "admin@smelite.bg";
            string adminPassword = "StrongPassword!123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Site",
                    LastName = "Admin",
                    Role = "Admin",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                // Можеш да добавиш logging на грешки при нужда
            }
        }

        public static async Task SeedCraftAndTrainingTypesAsync(ApplicationDbContext context)
        {
            if (!context.CraftTypes.Any())
            {
                var craftTypes = new List<CraftType>
                {
                    new CraftType { Name = "Woodworking" },
                    new CraftType { Name = "Metalworking" },
                    new CraftType { Name = "Pottery" },
                    new CraftType { Name = "Textiles" }
                };

                context.CraftTypes.AddRange(craftTypes);
            }

            if (!context.TrainingTypes.Any())
            {
                var trainingTypes = new List<TrainingType>
                {
                    new TrainingType { Name = "Online" },
                    new TrainingType { Name = "In-person" },
                    new TrainingType { Name = "Hybrid" }
                };

                context.TrainingTypes.AddRange(trainingTypes);
            }

            await context.SaveChangesAsync();
        }
    }
}
