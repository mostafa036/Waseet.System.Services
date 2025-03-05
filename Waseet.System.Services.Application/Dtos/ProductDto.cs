
using Microsoft.AspNetCore.Http;

namespace Waseet.System.Services.Application.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
