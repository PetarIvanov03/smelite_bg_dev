using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;
using smelite_app.Data;
using smelite_app.Filters;
using smelite_app.Models;
using smelite_app.Seed;

namespace smelite_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((context, services, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
                             .ReadFrom.Services(services)
                             .Enrich.FromLogContext()
                             .WriteTo.File(
                                 "Logs/log-.txt",
                                 rollingInterval: RollingInterval.Day,
                                 outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                             .WriteTo.Console(outputTemplate:
                                 "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));


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

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Error/AccessDenied";
                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    var user = ctx.HttpContext.User.Identity?.IsAuthenticated == true ? ctx.HttpContext.User.Identity?.Name : "Anonymous";
                    logger.LogWarning("Unauthorized access by {User} to {Path} on {Time}", user, ctx.Request.Path, DateTime.UtcNow);
                    ctx.Response.Redirect(options.AccessDeniedPath);
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<LogActionFilter>();
            });
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<Repositories.ICraftRepository, Repositories.CraftRepository>();
            builder.Services.AddScoped<Repositories.IApprenticeRepository, Repositories.ApprenticeRepository>();
            builder.Services.AddScoped<Repositories.IMasterRepository, Repositories.MasterRepository>();
            builder.Services.AddScoped<Repositories.IAccountRepository, Repositories.AccountRepository>();

            builder.Services.AddScoped<Services.ICraftService, Services.CraftService>();
            builder.Services.AddScoped<Services.IApprenticeService, Services.ApprenticeService>();
            builder.Services.AddScoped<Services.IMasterService, Services.MasterService>();
            builder.Services.AddScoped<Services.IAccountService, Services.AccountService>();
            builder.Services.AddScoped<Services.IAdminService, Services.AdminService>();
            builder.Services.AddTransient<Helpers.EmailSender>();
            builder.Services.AddScoped<LogActionFilter>();
            builder.Services.AddScoped<Services.IPaymentService, Services.PaymentService>();

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();

            // Seed роли и начален админ при старт
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Ensure database schema is up to date
                //context.Database.Migrate();

                await SeedData.SeedRolesAndAdminAsync(roleManager, userManager);
                await SeedData.SeedCraftAndTrainingTypesAsync(context);
                await SeedData.SeedDemoUsersAsync(userManager, context);
                await SeedData.SeedDemoOrdersAsync(context);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseExceptionHandler("/Error");

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
