using Waseet.System.Services.APIs.Helper;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.ImagePredictionServices;
using Waseet.System.Services.Application.IServices;
using Waseet.System.Services.Application.Resolving;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Infrastructure.Repositories;
using Waseet.System.Services.Infrastructure.Services;

namespace ShopSphere.Services.API.Extensions
{
    public static class ApplicationServicesExtenstions
    {
        public static IServiceCollection AddApplicationServicse(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISpecificationRepository<Product>, SpecificationRepository<Product>>();

            services.AddScoped<IBasketRepository,BasketRepository>();

            services.AddScoped<ITokenServices, TokenServices>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddTransient<ProductPictureResolver>();

            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();

            services.AddScoped<IImageService, ImageService>();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<IAIService, ImagePrediction>();

            services.AddScoped<IPhotoService, PhotoService>();

            //services.AddSingleton<IConfiguration>(provider => provider.GetRequiredService<IConfiguration>());


            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddAutoMapper(typeof(MappingProfiles));    

            return services;
        }
    }
}
