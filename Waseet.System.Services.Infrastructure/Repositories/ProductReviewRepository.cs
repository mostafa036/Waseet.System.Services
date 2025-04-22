using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Persistence.Data;

namespace Waseet.System.Services.Infrastructure.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {

        private readonly WaseetContext _context;
        private readonly IBaseRepository<ProductReview> _reviewRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductReviewRepository(WaseetContext context , IBaseRepository<ProductReview> reviewRepo , 
                                       UserManager<User> userManager , IMapper mapper , IConfiguration configuration)
        {
            _context = context;
            _reviewRepo = reviewRepo;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<List<ProductReview>> GetAllByIdAsync(int productId)
        {
            return await _context.Set<ProductReview>().Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<List<ProductReviewReturnDto>> GetReviewDtosByProductIdAsync(int productId)
        {
            var reviews = await GetAllByIdAsync(productId);

            var reviewDtos = new List<ProductReviewReturnDto>();

            foreach (var review in reviews)
            {
                var user = await _userManager.FindByEmailAsync(review.CustomerEamil);

                var reviewDto = _mapper.Map<ProductReviewReturnDto>(review);
                reviewDto.userId = user.Id;
                reviewDto.name = user?.DisplayName ?? "Unknown";
                reviewDto.profileImage = string.IsNullOrEmpty(user?.profileImage)
                    ? string.Empty
                    : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

                reviewDtos.Add(reviewDto);
            }
            return reviewDtos;
        }
    }
}