using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface IProductReviewRepository
    {
        Task<List<ProductReviewReturnDto>> GetReviewDtosByProductIdAsync(int productId);
        Task<List<ProductReview>> GetAllByIdAsync(int productId);
    }
}
