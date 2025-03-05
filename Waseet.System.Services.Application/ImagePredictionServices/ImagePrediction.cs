using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.IServices;

namespace Waseet.System.Services.Application.ImagePredictionServices
{
    public class ImagePrediction : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _aiApiUrl = "http://localhost:5000/predict"; // Python API URL

        public ImagePrediction(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string> AnalyzeImageAsync(IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = image.OpenReadStream();

            var fileContent = new StreamContent(fileStream);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(fileContent, "image", image.FileName);

            var response = await _httpClient.PostAsync(_aiApiUrl, content);
            if (!response.IsSuccessStatusCode) return "Error processing image.";

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);

            if (result.status == "bad")
                return $"Inappropriate content detected: {string.Join(", ", result.words)}";
            return "Image is clean.";
        }

    }
}
