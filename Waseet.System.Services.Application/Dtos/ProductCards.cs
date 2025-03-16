using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public class ProductCards
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string ImageURL { get; set; }
        public int Rating { get; set; }
        public string Category { get; set; }
        public string ServiceProviderImage { get; set; }
        public string ServiceProviderName { get; set; }
    }
}
