using Waseet.System.Services.Application.Filters;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Specifications;

namespace Waseet.System.Services.Infrastructure.SpecificationWithEntity
{
    // This class is used to get the product with its category
    public class ProductWithCategorySpec : BaseSpecification<Product>
    {
        public ProductWithCategorySpec(ProductFilterParams filterParams)
             : base(m =>
                  (string.IsNullOrEmpty(filterParams.ServiceProviderEmail) || m.ServiceProviderEmail.Contains(filterParams.ServiceProviderEmail))&&
                  ((!filterParams.CategoryId.HasValue || m.CategoryId == filterParams.CategoryId))
                 )
        {
            Includes.Add(x => x.Category);
            ApplyPaging(filterParams.PageSize * (filterParams.PageIndex - 1), filterParams.PageSize);
        }

        public ProductWithCategorySpec(int id) : base(x => x.Id == id)
        {
            Includes.Add(x => x.Category);
            Includes.Add(x => x.ProductReviews);
        }
    }
}