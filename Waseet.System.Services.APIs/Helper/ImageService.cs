using Waseet.System.Services.Application.Abstractions;

namespace Waseet.System.Services.APIs.Helper
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string folderPath)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Invalid image file");
            }

            // Ensure folder exists
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file asynchronously
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return relative path for storing in the database
            return $"/{folderPath}/{uniqueFileName}".Replace("\\", "/");
        }

    }

}
