using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models.Identity;

namespace Waseet.System.Services.Application.IServices
{
    public interface ITokenServices
    {
        Task<string> CreateToken(User user, UserManager<User> userManager);
    }
}
