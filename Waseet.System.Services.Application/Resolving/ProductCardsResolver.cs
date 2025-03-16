using AutoMapper;
using Microsoft.Extensions.Configuration;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Resolving
{
    public class ProductCardsResolver : IValueResolver<Product, ProductCards, string>
    {
        private readonly IConfiguration _configuration;

        public ProductCardsResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductCards destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageURL))
                return $"{_configuration["BaseApiUrl"]}{source.ImageURL}";
            return null;
        }
    }
}
