using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models.OrderAggeration
{
    public class DeliveryMethod : BaseEntity
    {

        public DeliveryMethod()
        {
            
        }

        public DeliveryMethod(string shortName, decimal cost, string description, string deliveryTime)
        {
            ShortName = shortName;
            Cost = cost;
            Description = description;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; } = string.Empty;
        public decimal Cost { get; set; } 
        public string Description { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;
    }
}
