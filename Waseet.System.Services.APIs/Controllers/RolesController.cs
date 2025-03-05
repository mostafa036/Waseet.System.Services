using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return BadRequest(new ApiResponse(400, "Role name cannot be empty"));

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (roleExists)
                return BadRequest(new ApiResponse(400, "Role already exists"));

            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
                return Ok($"Role '{roleName}' added successfully");
            else
                return BadRequest(new ApiResponse(400, "Error occurred while adding the role"));
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var roleNames = roles.Select(r => r.Name).ToList();

            return Ok(roleNames);
        }

        [HttpDelete("DeleteRole")]
        public async Task<ActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound("Role not found");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' deleted successfully.");
            }

            return BadRequest(new ApiResponse(400, $"Error occurred while deleting role {roleName}"));
        }

    }
}
