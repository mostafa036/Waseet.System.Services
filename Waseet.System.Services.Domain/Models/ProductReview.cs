
using System.ComponentModel.DataAnnotations;
using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models
{
    public class ProductReview : BaseEntity
    {
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string CustomerEamil { get; set; } = string.Empty;
    }
}
       // public string CustomerName { get; set; } = string.Empty;