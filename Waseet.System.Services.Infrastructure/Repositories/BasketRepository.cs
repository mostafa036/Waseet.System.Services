using StackExchange.Redis;
using System.Text.Json;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Domain.Models.OrderAggeration;

namespace Waseet.System.Services.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> CreateBasketIfNotExistsAsync(string basketId)
        {
            var existing = await GetBasketAsync(basketId);
            if (existing != null) return existing;

            var newBasket = new CustomerBasket(basketId);
            await database.StringSetAsync(basketId, JsonSerializer.Serialize(newBasket), TimeSpan.FromDays(30));
            return newBasket;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket newBasket)
        {
            var basket = await GetBasketAsync(newBasket.Id) ?? new CustomerBasket(newBasket.Id);

            foreach (var newItem in newBasket.Items)
            {
                var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == newItem.ProductId);
                if (existingItem != null)
                    existingItem.quantity += newItem.quantity;
                else
                    basket.Items.Add(newItem);
            }

            var saved = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            return saved ? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<bool> DeleteBasketItemAsync(string basketId, int productId)
        {
            var basket = await GetBasketAsync(basketId);
            if (basket == null) return false;

            var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return false;

            basket.Items.Remove(item);

            var updated = await database.StringSetAsync(basketId, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            return updated;
        }
    }
}
