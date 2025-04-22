using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public class BasketProductDto
    {
        public int Id { get; set; }               
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public string imageUrl { get; set; }
        public int Quantity { get; set; }       
    }

}
