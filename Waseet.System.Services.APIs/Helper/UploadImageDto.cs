using Microsoft.AspNetCore.Mvc;

namespace Waseet.System.Services.APIs.Helper
{
    public class UploadImageDto
    {
        [FromForm]
        public IFormFile File { get; set; }

        [FromForm]
        public int CategoryId { get; set; }
    }

}
