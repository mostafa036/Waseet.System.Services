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
        public int id { get; set; }
        public string userId { get; set; }
        public string comment { get; set; } = string.Empty;
        public int rating { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; } = null!;
        public string profileImage { get; set; } = null!;
    }
}
