using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models.OrderAggeration;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);
        Task<bool> DeleteBasketItemAsync(string basketId, int productId);
        Task<CustomerBasket> CreateBasketIfNotExistsAsync(string basketId);
    }

}
