
namespace Waseet.System.Services.Application.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string ImageURL { get; set; }
        public string ServiceProviderEmail { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
    }
}