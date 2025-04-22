using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {

        private readonly IBaseRepository<ProductReview> _reviewRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IProductReviewRepository _productReviewRepository;

        public ReviewController(IBaseRepository<ProductReview> reviewRepo,
                                UserManager<User> userManager , 
                                IMapper mapper , IConfiguration configuration ,
                                IProductReviewRepository productReviewRepository)
        {
            _reviewRepo = reviewRepo;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _productReviewRepository = productReviewRepository;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<ProductReviewReturnDto>> AddReview(ProductReviewDto reviewDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new ApiResponse(401, "User must be logged in to add a review."));

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return Unauthorized(new ApiResponse(401, "User not found."));
        
            var review = new ProductReview
            {
                Comment = reviewDto.comment,
                Rating = reviewDto.rating,
                ReviewDate = reviewDto.date,
                ProductId = reviewDto.productId,
                CustomerEamil = userEmail
            };

            await _reviewRepo.AddAsync(review);

            var reviewResponse = _mapper.Map<ProductReviewReturnDto>(review);
            reviewResponse.name = user.DisplayName;
            reviewResponse.userId = user.Id;

            reviewResponse.profileImage = string.IsNullOrEmpty(user.profileImage)
                 ? string.Empty
                 : $"{_configuration["BaseApiUrl"]}{user.profileImage}";
            return Ok(reviewResponse);
        }


        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new ApiResponse(401, "User must be logged in to delete a review."));

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return Unauthorized(new ApiResponse(401, "User not found."));

            var review = await _reviewRepo.GetByIdAsync(id);

            if (review == null)
                return NotFound(new ApiResponse(404, "Review not found."));

            if(review.CustomerEamil != userEmail)
                return NotFound(new ApiResponse(403, "You do not have permission to delete this review."));

            await _reviewRepo.DeleteAsync(review.Id);

            return Ok(new ApiResponse(200, "Review deleted successfully."));
        }


        [HttpGet("GetReviews/{productId}")]
        public async Task<ActionResult<List<ProductReviewReturnDto>>> GetReviewsForProduct(int productId)
        {
            var reviews = await _productReviewRepository.GetReviewDtosByProductIdAsync(productId);

            if (reviews == null || !reviews.Any())
                return NotFound(new ApiResponse(404, "No reviews found for this product."));

            return Ok(reviews);
        }


        [Authorize(Roles = "Customer")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<ProductReviewReturnDto>> UpdateReview(int id, ProductReviewDto reviewDto)
        {
            var review = await _reviewRepo.GetByIdAsync(id);
            if (review == null)
                return NotFound(new ApiResponse(404, "Review not found."));
            review.Comment = reviewDto.comment;
            review.Rating = reviewDto.rating;
            await _reviewRepo.UpdateAsync(review);
            var reviewResponse = _mapper.Map<ProductReviewReturnDto>(review);
            return Ok(reviewResponse);
        }
    }
}