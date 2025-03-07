using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Dtos
{
    public record GetCurrentUserdto(string DisplayName, string Password, string Token,string Roles);

}
