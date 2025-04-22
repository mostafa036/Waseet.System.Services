using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.APIs.Helper;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Application.Filters;
using Waseet.System.Services.Application.IServices;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;
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
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepositoryMapping;

        public ProductController(IBaseRepository<Product> productRepo , IMapper mapper , UserManager<User> userManager ,
                                 ISpecificationRepository<Product> specrepo , IConfiguration configuration  , 
                                 IProductRepository productRepositoryMapping )
        {
            _productRepo =  productRepo;
            _mapper = mapper;
            _userManager = userManager;
            _specrepo = specrepo;
            _configuration = configuration;
            _productRepositoryMapping = productRepositoryMapping;
        }

        [Authorize(Roles = "serviceProvider")]
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

            if (user == null || !await _userManager.IsInRoleAsync(user, "serviceProvider"))
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
                CategoryId = productDto.category,
                ImageURL = $"/ProductImages/{fileName}",
                ServiceProviderEmail = email
            };

            await _productRepo.AddAsync(product);

            string serviceProviderImg = string.IsNullOrEmpty(user.profileImage)
                                ? string.Empty
                                : $"{_configuration["BaseApiUrl"]}{user.profileImage}";


            string imageresolve = string.IsNullOrEmpty(product.ImageURL)
                     ? string.Empty
                     : $"{_configuration["BaseApiUrl"]}{product.ImageURL}";

            var response = new
            {
                Id = product.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                category = productDto.category,
                image = imageresolve,
                serviceProviderImg = serviceProviderImg,
                serviceProviderName = user.DisplayName
            };

            return Ok(response);
        }

        //[Authorize(Roles = "serviceProvider")]
        //[HttpPost("AddProduct")]
        //public async Task<ActionResult> CreateProduct([FromForm] ProductDto productDto,IFormFile image,[FromServices] IPhotoService photoService)
        //{
        //    if (productDto == null || image == null)
        //        return BadRequest(new ApiResponse(400, "Invalid request data or missing image."));

        //    var email = User.FindFirstValue(ClaimTypes.Email);

        //    if (string.IsNullOrEmpty(email))
        //        return Unauthorized(new ApiResponse(401));

        //    var user = await _userManager.FindByEmailAsync(email);

        //    if (user == null || !await _userManager.IsInRoleAsync(user, "serviceProvider"))
        //        return Forbid();

        //    // ✅ Upload to Cloudinary
        //    var uploadResult = await photoService.UploadImageAsync(image, "products");

        //    if (uploadResult.Error != null)
        //        return StatusCode(500, new ApiResponse(500, "Image upload failed."));

        //    var product = new Product
        //    {
        //        Name = productDto.Name,
        //        Description = productDto.Description,
        //        Price = productDto.Price,
        //        CategoryId = productDto.category,
        //        ImageURL = uploadResult.SecureUrl.ToString(), // ✅ Cloudinary URL
        //        ServiceProviderEmail = email
        //    };

        //    await _productRepo.AddAsync(product);

        //    string serviceProviderImg = string.IsNullOrEmpty(user.profileImage)
        //                            ? string.Empty
        //                            : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

        //    var imageResolved = string.IsNullOrEmpty(product.ImageURL)
        //                 ? string.Empty
        //                 : product.ImageURL; // Already full Cloudinary URL

        //    var response = new
        //    {
        //        Id = product.Id,
        //        Name = productDto.Name,
        //        Description = productDto.Description,
        //        Price = productDto.Price,
        //        category = productDto.category,
        //        image = imageResolved,
        //        serviceProviderImg = serviceProviderImg,
        //        serviceProviderName = user.DisplayName
        //    };

        //    return Ok(response);
        //}


        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            await _productRepo.DeleteAsync(product.Id);
            return Ok(new { message = "Product deleted successfully" });
        }

        // True
        [Authorize(Roles = "serviceProvider")]
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProductDTO productDto, IFormFile? image)
        {
            if (productDto == null)
                return BadRequest(new ApiResponse(400, "Invalid request data."));

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse(401));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "serviceProvider"))
                return Forbid(); // User is not a service provider

            var product = await _productRepo.GetByIdAsync(productDto.id);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product not found."));
            
            if (image != null)
            {
                var fileName = await SaveImageAsync(image);
                if (!string.IsNullOrEmpty(fileName))
                {
                    product.ImageURL = $"/ProductImages/{fileName}";
                }
                else
                {
                    return StatusCode(500, new ApiResponse(500, "Error saving image."));
                }
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.OldPrice = productDto.OldPrice;
            product.CategoryId = productDto.category;

            await _productRepo.UpdateAsync(product);

            string serviceProviderImg = string.IsNullOrEmpty(user.profileImage)
                ? string.Empty
                : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

            string imageresolve = string.IsNullOrEmpty(product.ImageURL)
                ? string.Empty
                : $"{_configuration["BaseApiUrl"]}{product.ImageURL}";

            var response = new
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                OldPrice = product.OldPrice,
                image = imageresolve,
                serviceProviderImg = serviceProviderImg,
                serviceProviderName = user.DisplayName
            };
            return Ok(response);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {

            var spec = new ProductWithCategorySpec(id);

            var data = await _specrepo.GetEntityWithSpecAsync(spec);

            var user = await _userManager.FindByEmailAsync(data.ServiceProviderEmail);

            if (data == null) return NotFound(new ApiResponse(404));

            var result = _mapper.Map<ProductToReturnDto>(data);

            result.ServiceProviderName = user.DisplayName;
            result.ServiceProviderImage = string.IsNullOrEmpty(user.profileImage)
                ? string.Empty
                : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

            return Ok(result);
        }

        [HttpGet("ProductsCards")]
        public async Task<ActionResult<Pagination<ProductCards>>> GetProducts([FromQuery] ProductFilterParams productFilter)
        {
            string? userEmail = null;
            bool isServiceProvider = false;

            // Check if the user is authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                userEmail = User.FindFirstValue(ClaimTypes.Email);
                isServiceProvider = User.IsInRole("serviceProvider");

                if (isServiceProvider)
                {
                    // Only apply the filter if the user is a service provider
                    productFilter.ServiceProviderEmail = userEmail;
                }
            }

            var spec = new ProductWithCategorySpec(productFilter);
            var products = await _specrepo.GetAllWithSpecAsync(spec);

            if (products == null || !products.Any())
                return NotFound(new ApiResponse(404, "No products found."));

            var mappedResult = await _productRepositoryMapping.GetProductCardsWithServicProvider(products.ToList());

            var countSpec = new ProductsWithFiltersForCountSpecification(productFilter);

            var count = await _specrepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductCards>(productFilter.PageIndex, productFilter.PageSize, count, mappedResult));
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