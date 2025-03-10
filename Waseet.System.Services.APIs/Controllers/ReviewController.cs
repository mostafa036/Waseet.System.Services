using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Identity;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IBaseRepository<ProductReview> _reviewRepo;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ReviewController(IBaseRepository<ProductReview> reviewRepo, UserManager<User> userManager , IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _userManager = userManager;
            _mapper = mapper;
        }


        [Authorize(Roles = "Customer")]
        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] ProductReviewDto reviewDto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("User must be logged in to add a review.");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Unauthorized("User not found.");

            
            var review = new ProductReview
            {
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                ReviewDate = DateTime.UtcNow,
                ProductId = reviewDto.ProductId,
                CustomerEamil = user.Email 
            };

            await _reviewRepo.AddAsync(review);

            //var result = new ProductReviewReturnDto
            //{
            //    Comment = review.Comment,
            //    Rating = review.Rating,
            //    ReviewDate = review.ReviewDate,
            //    ProductId = review.ProductId,
            //    CustomerName = review.CustomerEamil,
            //    CustomerImage = user.profilePhotoesPath,
            //};



            var reviewResponse = _mapper.Map<ProductReviewReturnDto>(review);

            reviewResponse.CustomerName = user.DisplayName;

            reviewResponse.CustomerImage = user.profilePhotoesPath;

            return Ok();
        }

        //[HttpGet("GetReviews/{productId}")]
        //public async Task<IActionResult> GetReviews(int productId)
        //{
        //    var reviews = await _reviewRepo.GetAllAsync(r => r.ProductId == productId);
        //    return Ok(reviews);
        //}

    }
}
