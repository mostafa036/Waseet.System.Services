using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.System.Services.Application.IServices;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;

        public AIController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Invalid image file.");

            var result = await _aiService.AnalyzeImageAsync(image);
            return Ok(new { message = result });
        }
    }
}
