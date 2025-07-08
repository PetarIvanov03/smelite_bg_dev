using Microsoft.AspNetCore.Identity;
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
                    FullName = "Site Admin",
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
    }
}
