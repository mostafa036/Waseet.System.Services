using AutoMapper;
using Microsoft.Extensions.Configuration;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Resolving
{
    public class CategoryPictureResolver : IValueResolver<Category, CategoryDto, string>
    {

        private readonly IConfiguration _configuration;

        public CategoryPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Category source, CategoryDto destination, string destMember, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source.imagUrl)
                ? string.Empty
                : $"{_configuration["BaseApiUrl"]}{source.imagUrl}";
        }
    }    
}