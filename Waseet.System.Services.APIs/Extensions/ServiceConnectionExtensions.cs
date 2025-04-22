
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
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

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
                config.AllowAdmin = true;
                return ConnectionMultiplexer.Connect(config);
            });

        }
    }
}
