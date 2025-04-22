using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Persistence.Data;
using Waseet.System.Services.Persistence.Data.DataSeeding;

namespace Waseet.System.Services.APIs.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task ApplyMigrationsAndSeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var scopedServices = scope.ServiceProvider;

            var logger = scopedServices.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

            try
            {
                // Seed User data
                var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
                // Seed users and roles
                await UserIdentityDbContextSeed.SeedAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration and data seeding.");
            }
        }




    }
}
