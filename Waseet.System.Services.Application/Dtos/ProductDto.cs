
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Waseet.System.Services.Application.Dtos
{
    public class  ProductDto
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        [Required]
        public int category { get; set; }
    }
}
