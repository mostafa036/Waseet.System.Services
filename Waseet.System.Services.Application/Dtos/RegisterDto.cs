using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public record RegisterDto(
    
     [Required]
    string DisplayName,
      [Required]
    [EmailAddress]
    string Email,
      [Required]
    [StringLength(100, MinimumLength = 8)]
    //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)d@$!%*?&]",
    //ErrorMessage = "Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one digit, and may contain special characters.")]
    string Password,
    string role
      );
}
