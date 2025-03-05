using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public record UserDto (string DisplayName , string Password , string Token);

    public record LoginDto([Required] [EmailAddress]string Email, [Required]string Password);
}
