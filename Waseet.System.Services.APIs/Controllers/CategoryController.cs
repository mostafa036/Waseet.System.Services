using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Waseet.System.Services.APIs.Helper;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Application.Resolving;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IBaseRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryController(IBaseRepository<Category> CategoryRepo , IMapper mapper)
        {
            _categoryRepo = CategoryRepo;
            _mapper = mapper;
        }


        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            var result = await _categoryRepo.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("Category/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<Category>> UpdateCategory(Category category)
        {
            var result = await _categoryRepo.UpdateAsync(category);
            return Ok(result);
        }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            var result = await _categoryRepo.AddAsync(category);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDto model)
        {
            if (!IsValidImage(model.File))
            {
                return BadRequest("Invalid file. Please upload a valid image.");
            }

            var category = await _categoryRepo.GetByIdAsync(model.CategoryId);

            if (category == null)
            {
                return BadRequest("Invalid category.");
            }

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Category");
            Directory.CreateDirectory(uploadDirectory);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Save the relative URL to the category
            category.imagUrl = $"/Category/{uniqueFileName}";
            await _categoryRepo.UpdateAsync(category);

            return Ok(new { path = category.imagUrl });
        }

        private bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }
    }
}