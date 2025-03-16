using AutoMapper;
using Microsoft.Extensions.Configuration;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models.Identity;

namespace Waseet.System.Services.Application.Resolving
{
    public class ProductReviewResolver : IValueResolver<User, ProductReviewReturnUserData, string>
    {
        private readonly IConfiguration _configuration;

        public ProductReviewResolver(IConfiguration configuration )
        {

            _configuration = configuration;

        }

        public string Resolve(User source, ProductReviewReturnUserData destination, string destMember, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source.profileImage)
                ? string.Empty
                : $"{_configuration["BaseApiUrl"]}{source.profileImage}";
        }

    }
}