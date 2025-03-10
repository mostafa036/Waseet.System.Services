using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public class ProductReviewReturnDto
    {
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public DateTime ReviewDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerImage { get; set; } = string.Empty;
    }
}
