using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Persistence.Data;

namespace Waseet.System.Services.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly WaseetContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ProductRepository(WaseetContext context, IConfiguration configuration, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
        }

        public Task<List<ProductToReturnDto>> GetAllByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductCards>> GetProductCardsWithServicProvider(List<Product> Models)
        {

            var productCards = new List<ProductCards>();

            foreach (var product in Models)
            {
                var user = await _userManager.FindByEmailAsync(product.ServiceProviderEmail);

                var products = _mapper.Map<ProductCards>(product);

                products.ImageURL = string.IsNullOrEmpty(product.ImageURL)
                    ? string.Empty
                    : $"{_configuration["BaseApiUrl"]}{product.ImageURL}";

                products.ServiceProviderName = user?.DisplayName ?? "Unknown";

                products.ServiceProviderImage = string.IsNullOrEmpty(user?.profileImage)
                    ? string.Empty
                    : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

                productCards.Add(products);
            }
            return productCards;
        }

        public async Task<List<Product>> GetProductsByServiceProviderEmail(string email)
        {
            var products = await _context.products
                          .Where(p => p.ServiceProviderEmail == email).ToListAsync();

            return products;
        }
    }
}
