using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models.Identity;

namespace Waseet.System.Services.Application.Resolving
{
    public class ProductCardsServicesProviderResolver : IValueResolver<User, ProductCardsReturnUserData, string>
    {

        private readonly IConfiguration _configuration;

        public ProductCardsServicesProviderResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(User source, ProductCardsReturnUserData destination, string destMember, ResolutionContext context)
        {
            return string.IsNullOrEmpty(source.profileImage)
                ? string.Empty
                : $"{_configuration["BaseApiUrl"]}{source.profileImage}";
        }
    }
}
