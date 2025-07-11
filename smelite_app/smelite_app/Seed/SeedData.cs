using Microsoft.AspNetCore.Identity;
using smelite_app.Data;
using smelite_app.Models;
using smelite_app.Helpers;
using Microsoft.EntityFrameworkCore;

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

            }
        }

        public static async Task SeedCraftAndTrainingTypesAsync(ApplicationDbContext context)
        {
            if (!context.CraftTypes.Any())
            {
                var craftTypes = new List<CraftType>
                {
                    new CraftType { Name = "Ножарство" },
                    new CraftType { Name = "Грънчарство" },
                    new CraftType { Name = "Дърводелство" },
                    new CraftType { Name = "Дърворезбарство" },
                    new CraftType { Name = "Златарство" },
                    new CraftType { Name = "Леярство" },
                    new CraftType { Name = "Керамика" },
                    new CraftType { Name = "Обработка на художествени тъкани" },
                    new CraftType { Name = "Гайтанджийство" },
                    new CraftType { Name = "Изработване на национални костюми" },
                    new CraftType { Name = "Изработване на накити" },
                    new CraftType { Name = "Пафти - куюмджийство" },
                    new CraftType { Name = "Изработка и ремонт на музикални инструменти" },
                    new CraftType { Name = "Бакърджийство" },
                    new CraftType { Name = "Килимарство" },
                    new CraftType { Name = "Копаничарство" },
                    new CraftType { Name = "Изработване на стъклени изделия чрез духане на стъкло" },
                    new CraftType { Name = "Рисуване и гравиране върху стъкло" },
                    new CraftType { Name = "Ръчно книговезване" },
                    new CraftType { Name = "Часовникарство" },
                    new CraftType { Name = "Гравьорство" },
                    new CraftType { Name = "Коминочистене" },
                    new CraftType { Name = "Тенекеджийство" },
                    new CraftType { Name = "Калайджийство" },
                    new CraftType { Name = "Ковачество" },
                    new CraftType { Name = "Каменоделство" },
                    new CraftType { Name = "Точиларство" },
                    new CraftType { Name = "Бъчварство" },
                    new CraftType { Name = "Кошничарство, изработване на рогозки и метли" },
                    new CraftType { Name = "Тъкачество (платно, аби, пътеки, черги, китеници и сродни) и мутафчийство" },
                    new CraftType { Name = "Седларство и сарачество" },
                    new CraftType { Name = "Въжарство" },
                    new CraftType { Name = "Овощарство" },
                    new CraftType { Name = "Винарство" },
                    new CraftType { Name = "Пчеларство" },
                    new CraftType { Name = "Медарство" },
                    new CraftType { Name = "Млекарство" },
                    new CraftType { Name = "Животновъдство - овце, крави, кози, коне, прасета" },
                    new CraftType { Name = "Производство на боза (бозаджийство) и шекерджийство, производство на изделия от захарен сироп" }
                };

                context.CraftTypes.AddRange(craftTypes);
            }

            if (!context.CraftLocations.Any())
            {
                var craftLocations = new List<CraftLocation>
                {
                    new CraftLocation { Name = "Online" },
                    new CraftLocation { Name = "In-person" },
                    new CraftLocation { Name = "Hybrid" }
                };

                context.CraftLocations.AddRange(craftLocations);
            }

            if (!context.CraftPackages.Any())
            {
                var craftPackages = new List<CraftPackage>
                {
                    new CraftPackage { SessionsCount = 1 },
                    new CraftPackage { SessionsCount = 5 },
                    new CraftPackage { SessionsCount = 10 }
                };

                context.CraftPackages.AddRange(craftPackages);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedDemoUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            MasterProfile? masterProfile = null;

            if (!userManager.Users.Any(u => u.Email == "master@demo.bg"))
            {
                var masterUser = new ApplicationUser
                {
                    UserName = "master@demo.bg",
                    Email = "master@demo.bg",
                    FirstName = "Demo",
                    LastName = "Master",
                    Role = "Master",
                    EmailConfirmed = true,
                    ProfileImageUrl = Variables.defaultProfileImageUrl
                };
                await userManager.CreateAsync(masterUser, "Password123!");
                await userManager.AddToRoleAsync(masterUser, "Master");
                masterProfile = new MasterProfile { ApplicationUserId = masterUser.Id, PersonalInformation = "Demo master" };
                context.MasterProfiles.Add(masterProfile);
            }
            else
            {
                masterProfile = await context.MasterProfiles.Include(mp => mp.ApplicationUser)
                    .FirstOrDefaultAsync(mp => mp.ApplicationUser.Email == "master@demo.bg");
            }

            if (!userManager.Users.Any(u => u.Email == "apprentice@demo.bg"))
            {
                var apprenticeUser = new ApplicationUser
                {
                    UserName = "apprentice@demo.bg",
                    Email = "apprentice@demo.bg",
                    FirstName = "Demo",
                    LastName = "Apprentice",
                    Role = "Apprentice",
                    EmailConfirmed = true,
                    ProfileImageUrl = Variables.defaultProfileImageUrl
                };
                await userManager.CreateAsync(apprenticeUser, "Password123!");
                await userManager.AddToRoleAsync(apprenticeUser, "Apprentice");
                context.ApprenticeProfiles.Add(new ApprenticeProfile { ApplicationUserId = apprenticeUser.Id, PersonalInformation = "Demo apprentice" });
            }

            await context.SaveChangesAsync();

            if (masterProfile != null && !context.MasterProfileCrafts.Any(mpc => mpc.MasterProfileId == masterProfile.Id))
            {
                var typeIds = context.CraftTypes.Take(2).Select(ct => ct.Id).ToList();
                var crafts = new List<Craft>
                {
                    new Craft { Name = "Demo Craft 1", CraftDescription = "Seeded craft", ExperienceYears = 5, CraftTypeId = typeIds[0] },
                    new Craft { Name = "Demo Craft 2", CraftDescription = "Seeded craft", ExperienceYears = 3, CraftTypeId = typeIds[1] }
                };

                context.Crafts.AddRange(crafts);
                await context.SaveChangesAsync();

                foreach (var craft in crafts)
                {
                    context.MasterProfileCrafts.Add(new MasterProfileCraft { MasterProfileId = masterProfile.Id, CraftId = craft.Id });
                    context.CraftImages.Add(new CraftImage { CraftId = craft.Id, ImageUrl = Variables.defaultCraftImageUrl });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
