using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models.OrderAggeration;

namespace Waseet.System.Services.Application.Dtos
{
    public class CustomerBasketDto
    {
        public List<BasketItem> Items { get; set; }
    }
}
