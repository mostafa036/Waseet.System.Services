using System.ComponentModel.DataAnnotations;

namespace Waseet.System.Services.Application.Dtos
{
    public class ProductReviewDto
    {
        public string comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int rating { get; set; }
        public int productId { get; set; }
        public DateTime date { get; set; } 
    }
}
