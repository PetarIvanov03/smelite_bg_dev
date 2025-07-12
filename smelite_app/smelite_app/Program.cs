using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;
using smelite_app.Seed;

namespace smelite_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // ВАЖНО: Тук използвай твоя ApplicationUser, не дефолтния IdentityUser!    
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<Repositories.ICraftRepository, Repositories.CraftRepository>();
            builder.Services.AddScoped<Repositories.IApprenticeRepository, Repositories.ApprenticeRepository>();
            builder.Services.AddScoped<Repositories.IMasterRepository, Repositories.MasterRepository>();

            builder.Services.AddScoped<Services.ICraftService, Services.CraftService>();
            builder.Services.AddScoped<Services.IApprenticeService, Services.ApprenticeService>();
            builder.Services.AddScoped<Services.IMasterService, Services.MasterService>();
            builder.Services.AddScoped<Services.IAccountService, Services.AccountService>();

            var app = builder.Build();

            // Seed роли и начален админ при старт
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await SeedData.SeedRolesAndAdminAsync(roleManager, userManager);
                await SeedData.SeedCraftAndTrainingTypesAsync(context);
                await SeedData.SeedDemoUsersAsync(userManager, context);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        // Seed-ване на роли и начален админ акаунт

    }
}
