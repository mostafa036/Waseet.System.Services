using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;
        public string imagUrl { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
