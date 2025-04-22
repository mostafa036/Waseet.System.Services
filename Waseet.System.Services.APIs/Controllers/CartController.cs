using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Domain.Models.OrderAggeration;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<Product> _productrepo;
        private readonly IConfiguration _configuration;

        public CartController(IBasketRepository basketRepository, 
                              IMapper mapper , UserManager<User> userManager,
                              IBaseRepository<Product> productrepo,
                              IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _userManager = userManager;
            _productrepo = productrepo;
            _configuration = configuration;
        }


        [Authorize]
        [HttpGet("basket")]
        [ProducesResponseType(typeof(List<BasketProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<BasketProductDto>>> GetUserBasket()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiResponse(401));
            var user =  await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var basket = await _basketRepository.CreateBasketIfNotExistsAsync(user.Id);
            var response = new List<BasketProductDto>();

            foreach (var item in basket.Items)
            {
                var product = await _productrepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    continue;

                var image = string.IsNullOrEmpty(product.ImageURL)
                    ? string.Empty
                    : $"{_configuration["BaseApiUrl"]}{product.ImageURL}";

                response.Add(new BasketProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    imageUrl = image,
                    Quantity = item.quantity
                });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("basket")]
        [ProducesResponseType(typeof(List<BasketProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<BasketProductDto>>> UpdateBasket([FromBody] List<BasketItem> items)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiResponse(401));
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var updatedBasket = new CustomerBasket(user.Id) { Items = items };
            var result = await _basketRepository.UpdateBasketAsync(updatedBasket);
            if (result == null)
                return BadRequest(new ApiResponse(400, "Failed to update basket"));

            var response = new List<BasketProductDto>();
            foreach (var item in result.Items)
            {
                var product = await _productrepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    continue;

                var image = string.IsNullOrEmpty(product.ImageURL)
                    ? string.Empty
                    : $"{_configuration["BaseApiUrl"]}{product.ImageURL}";

                response.Add(new BasketProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    imageUrl = image,
                    Quantity = item.quantity
                });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("basket/item/{productId}")]
        public async Task<IActionResult> DeleteBasketItem(int productId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            var deleted = await _basketRepository.DeleteBasketItemAsync(user.Id, productId);
            if (!deleted)
                return NotFound(new ApiResponse(404, "Item not found in basket"));

            return NoContent();
        }

        [Authorize]
        [HttpDelete("basket")]
        public async Task<IActionResult> DeleteBasket()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiResponse(401));
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            if (user == null)
                return Unauthorized(new ApiResponse(401));

            await _basketRepository.DeleteBasketAsync(user.Id);
            return NoContent();
        }
    }
}
