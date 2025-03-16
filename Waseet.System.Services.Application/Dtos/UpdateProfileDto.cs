using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public class UpdateProfileDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public IFormFile? profileImage { get; set; }
        public string? profession { get; set; }
        public string? bio { get; set; }
    }
}
