using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string? ImageURL { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string ServiceProviderEmail { get; set; } = string.Empty;

        public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    }
}