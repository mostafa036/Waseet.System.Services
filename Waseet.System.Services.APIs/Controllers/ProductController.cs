using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBaseRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductController(IBaseRepository<Product> productRepo , IMapper mapper)
        {
            _productRepo =  productRepo;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("AddProduct")]
        public async Task<ActionResult> CreateProduct([FromForm] ProductDto productDto, [FromForm] IFormFile image)
        {

            if (productDto == null || image == null)
                return BadRequest("Invalid request data or missing image.");

            // Generate unique image filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine("wwwroot/ProductImages", fileName);

            // Save image to server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                OldPrice = productDto.OldPrice,
                CategoryId = productDto.CategoryId,
                ImageURL = $"/ProductImages/{fileName}" // Save relative path
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
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
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
    }
}
