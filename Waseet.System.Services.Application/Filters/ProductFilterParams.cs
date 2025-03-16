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
        public int? CategoryId { get; set; }
        public string? ServiceProviderEmail { get; set; }


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
