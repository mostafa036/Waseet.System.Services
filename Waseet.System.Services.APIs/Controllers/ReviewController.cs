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


        public ReviewController(IBaseRepository<ProductReview> reviewRepo, UserManager<User> userManager , 
                                IMapper mapper , IConfiguration configuration , IProductReviewRepository productReviewRepository)
        {
            _reviewRepo = reviewRepo;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _productReviewRepository = productReviewRepository;
        }


        [Authorize(Roles = "customer")]
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
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                ReviewDate = DateTime.UtcNow,
                ProductId = reviewDto.ProductId,
                CustomerEamil = user.Email
            };

            await _reviewRepo.AddAsync(review);

            var reviewResponse = _mapper.Map<ProductReviewReturnDto>(review);

            reviewResponse.CustomerName = user.DisplayName;

            reviewResponse.CustomerImage = string.IsNullOrEmpty(user.profileImage)
                 ? string.Empty
                 : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

            return Ok(reviewResponse);
        }


        [Authorize(Roles = "Admin,Customer")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewRepo.GetByIdAsync(id);

            if (review == null)
                return NotFound(new ApiResponse(404, "Review not found."));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (review.CustomerEamil != userEmail)
                return Unauthorized(new ApiResponse(401, "You are not authorized to delete this review."));

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
            review.Comment = reviewDto.Comment;
            review.Rating = reviewDto.Rating;
            await _reviewRepo.UpdateAsync(review);
            var reviewResponse = _mapper.Map<ProductReviewReturnDto>(review);
            return Ok(reviewResponse);
        }

    }
}