using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public class CategoryCreateDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public IFormFile ImageUrl { get; set; } 
    }
}
