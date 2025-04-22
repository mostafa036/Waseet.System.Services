using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Common;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface IProductRepository
    {
        Task<List<ProductCards>> GetProductCardsWithServicProvider(List<Product> Models);
        Task<List<ProductToReturnDto>> GetAllByIdAsync(int productId);
        Task<List<Product>> GetProductsByServiceProviderEmail(string email);
    }
}
