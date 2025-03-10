using AutoMapper;
using Microsoft.Extensions.Configuration;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Resolving
{
    public class ProductPictureResolver : IValueResolver<Product, ProductToReturnDto, string>
    {

        private readonly IConfiguration _configuration;

        public ProductPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageURL))
                return $"{_configuration["BaseApiUrl"]}{source.ImageURL}";
             return null;
        }
    }
}