using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.Filters;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Specifications;

namespace Waseet.System.Services.Infrastructure.SpecificationWithEntity
{
    public class ProductsWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpecification(ProductFilterParams filterParams)
               : base(m =>
                  (string.IsNullOrEmpty(filterParams.ServiceProviderEmail) || m.ServiceProviderEmail.Contains(filterParams.ServiceProviderEmail)) &&
                  ((!filterParams.CategoryId.HasValue || m.CategoryId == filterParams.CategoryId) &&
                  (string.IsNullOrEmpty(filterParams.Search) || m.Description.ToLower().Contains(filterParams.Search.ToLower()) ||
                   m.Name.ToLower().Contains(filterParams.Search.ToLower())) &&
                  (!filterParams.min.HasValue || m.Price >= filterParams.min) &&
                  (!filterParams.max.HasValue || m.Price <= filterParams.max)
             ))
        {
        }
    }
}
