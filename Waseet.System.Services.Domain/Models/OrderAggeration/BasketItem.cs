using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models.OrderAggeration
{
    public class BasketItem 
    {
        //public int Id { get; set; }
        public int ProductId { get; set; }
        public int quantity { get; set; } = 1;
    }
}
