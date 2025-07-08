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
