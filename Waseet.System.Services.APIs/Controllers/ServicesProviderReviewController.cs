using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesProviderReviewController : ControllerBase
    {
        private readonly IBaseRepository<ServicesProviderReview> _baseRepository;

        public ServicesProviderReviewController(IBaseRepository<ServicesProviderReview> baseRepository )
        {
            _baseRepository = baseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _baseRepository.GetAllAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _baseRepository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ServicesProviderReview servicesProviderReview)
        {
            var result = await _baseRepository.AddAsync(servicesProviderReview);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ServicesProviderReview servicesProviderReview)
        {
            var result = await _baseRepository.UpdateAsync(servicesProviderReview);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _baseRepository.DeleteAsync(id);
            return Ok(result);
        }

    }
}
