using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public record UserDto (string DisplayName , string Email , string Token);

    public record LoginDto([Required] string Email, [Required]string Password);
}
