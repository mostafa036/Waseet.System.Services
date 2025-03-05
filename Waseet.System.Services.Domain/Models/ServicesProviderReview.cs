
using System.ComponentModel.DataAnnotations;
using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models
{
    public class ServicesProviderReview : BaseEntity
    {
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ServiceProviderEmail { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
