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
              :base(m =>
                  (string.IsNullOrEmpty(filterParams.ServiceProviderEmail) || m.ServiceProviderEmail.Contains(filterParams.ServiceProviderEmail))&&
                  ((!filterParams.CategoryId.HasValue || m.CategoryId == filterParams.CategoryId))
                 )
        {

        }
    }
}
