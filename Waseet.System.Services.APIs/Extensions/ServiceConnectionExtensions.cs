
using Microsoft.EntityFrameworkCore;
using Waseet.System.Services.Persistence.Data;

namespace ShopSphere.Services.API.Extensions
{
    public static class ServiceConnectionExtensions
    {
        public static void ConfigureDatabases( this IServiceCollection services , IConfiguration configuration)
        {

            services.AddDbContext<WaseetContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<UserIdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
        }
    }
}
