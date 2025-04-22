using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesProviderController : ControllerBase
    {
        private readonly IBaseRepository<Product> _baseRepository;
        private readonly UserManager<User> _userManager;

        public ServicesProviderController(IBaseRepository<Product> baseRepository , UserManager<User> userManager)
        {
            _baseRepository = baseRepository;
            _userManager = userManager;
        }








    }
}
