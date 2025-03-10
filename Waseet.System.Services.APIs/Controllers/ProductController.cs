using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Identity;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Infrastructure.SpecificationWithEntity;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBaseRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ISpecificationRepository<Product> _specrepo;

        public ProductController(IBaseRepository<Product> productRepo , IMapper mapper , UserManager<User> userManager ,ISpecificationRepository<Product> specrepo)
        {
            _productRepo =  productRepo;
            _mapper = mapper;
            _userManager = userManager;
            _specrepo = specrepo;
        }

        [Authorize(Roles = "serviceprovider")]
        [HttpPost("AddProduct")]
        public async Task<ActionResult> CreateProduct([FromForm] ProductDto productDto, IFormFile image)
        {
            if (productDto == null || image == null)
                return BadRequest(new ApiResponse(400, "Invalid request data or missing image."));

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new ApiResponse(401));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.IsInRoleAsync(user, "serviceprovider"))
                return Forbid(); // User is not a service provider

            var fileName = await SaveImageAsync(image);
            if (string.IsNullOrEmpty(fileName))
            {
                return StatusCode(500, new ApiResponse(500, "Error saving image."));
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                OldPrice = productDto.OldPrice,
                CategoryId = productDto.CategoryId,
                ImageURL = $"/ProductImages/{fileName}",
                ServiceProviderEmail = email
            };

            await _productRepo.AddAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }



        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            await _productRepo.DeleteAsync(product.Id);
            return Ok(new { message = "Product deleted successfully" });
        }



        // Update a product
        [HttpPut("UpdateProduct/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.CategoryId = productDto.CategoryId;

            await _productRepo.UpdateAsync(product);
            return Ok(new { message = "Product updated successfully", product });
        }



        // Get a product by ID
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {

            var spec = new ProductWithCategorySpec(id);

            var data = await _specrepo.GetEntityWithSpecAsync(spec);

            if (data == null) return NotFound(new ApiResponse(404));

            var result = _mapper.Map<ProductToReturnDto>(data);

            return Ok(result);
        }



        // Get all products
        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepo.GetAllAsync();
            return Ok(products);
        }



        [HttpPost("UploadProductImage/{productId}")]
        public async Task<ActionResult<string>> UploadProductImage(int productId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid image file.");
            }

            // Check if the product exists
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Define the product images folder
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "product");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Store image URL in the product entity
            product.ImageURL = $"/product/{fileName}";
            await _productRepo.UpdateAsync(product);

            return Ok(new { ImageURL = product.ImageURL });
        }


        /// <summary>
        /// Saves an uploaded image and returns the filename.
        /// </summary>
        

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine("wwwroot/ProductImages", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Ensure directory exists

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}