using System.ComponentModel.DataAnnotations;

namespace Waseet.System.Services.Application.Dtos
{
    public class ProductReviewDto
    {
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }
        public int ProductId { get; set; }
    }
}
