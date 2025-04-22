using Waseet.System.Services.Domain.Common;
using Waseet.System.Services.Domain.Enums;

namespace Waseet.System.Services.Domain.Models.OrderAggeration
{
    //public class Order : BaseEntity
    //{
    //    public Order() { }

    //    public Order(string buyerEmail, Address shippingToAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems , decimal subtotal)
    //    {
    //        BuyerEmail = buyerEmail;
    //        ShippingToAddress = shippingToAddress;
    //        DeliveryMethod = deliveryMethod;
    //        OrderItems = orderItems;
    //        Subtotal = subtotal;
    //    }

    //    public string BuyerEmail { get; set; } = string.Empty;
    //    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    //    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    //    public Address ShippingToAddress { get; set; } 
    //    public DeliveryMethod DeliveryMethod { get; set; } // Navigational property [One]
    //    public ICollection<OrderItem> OrderItems { get; set; } // Navigational property [Many]
    //    public decimal Total { get; set; }
    //    public decimal Subtotal { get; set; }
    //    public string PaymentIntentId { get; set; } = string.Empty;
    //    public decimal GetTotal() 
    //        => Subtotal + DeliveryMethod.Cost;
    //}
}
