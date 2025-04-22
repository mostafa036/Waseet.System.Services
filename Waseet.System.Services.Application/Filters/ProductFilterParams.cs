using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.Application.Filters
{
    public class ProductFilterParams
    {
        public string? Price { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public string? ServiceProviderEmail { get; set; }

        public decimal? min { get; set; }  // Minimum price for filtering
        public decimal? max { get; set; }  // Maximum price for filtering


        private const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
