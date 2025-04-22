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
                  ((!filterParams.CategoryId.HasValue || m.CategoryId == filterParams.CategoryId) &&
                  (string.IsNullOrEmpty(filterParams.Search) ||m.Description.ToLower().Contains(filterParams.Search.ToLower()) ||
                   m.Name.ToLower().Contains(filterParams.Search.ToLower())) &&
                  (!filterParams.min.HasValue || m.Price >= filterParams.min) &&
                  (!filterParams.max.HasValue || m.Price <= filterParams.max)
             ))
        {
            Includes.Add(x => x.Category);

            // Sorting logic
            if (!string.IsNullOrEmpty(filterParams.Sort))
            {
                switch (filterParams.Sort.ToLower())
                {
                    case "priceasc":
                        ApplyOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        ApplyOrderByDescending(p => p.Price);
                        break;
                    case "nameasc":
                        ApplyOrderBy(p => p.Name);
                        break;
                    case "namedesc":
                        ApplyOrderByDescending(p => p.Name);
                        break;
                }
            }

            // Pagination
            ApplyPaging(filterParams.PageSize * (filterParams.PageIndex - 1), filterParams.PageSize);
        }

        public ProductWithCategorySpec(int id) : base(x => x.Id == id)
        {
            Includes.Add(x => x.Category);
            Includes.Add(x => x.ProductReviews);
        }
    }
}