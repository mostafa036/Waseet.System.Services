using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Filters
{
    public class ProductFilterParams
    {



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
