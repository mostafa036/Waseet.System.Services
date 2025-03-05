
namespace Waseet.System.Services.Application.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string? ImageURL { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int rating { get; set; }
        public string Category { get; set; } = string.Empty;
        public UserReturnDto ServiceProvider { get; set; } = null!;
    }
}