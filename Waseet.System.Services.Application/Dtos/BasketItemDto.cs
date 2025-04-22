using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public int quantity { get; set; }
    }
}
