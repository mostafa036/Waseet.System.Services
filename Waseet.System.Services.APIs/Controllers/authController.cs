using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Waseet.System.Services.Application.Abstractions;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Application.IServices;
using Waseet.System.Services.Domain.Models.Identity;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public authController(UserManager<User> userManager, SignInManager<User> signInManager,
                              ITokenServices tokenServices, RoleManager<IdentityRole> roleManager,
                              IImageService imageService , IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _roleManager = roleManager;
            _imageService = imageService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) 
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            if (await _userManager.IsLockedOutAsync(user))
                return Unauthorized(new ApiResponse(401, "Your account is locked due to multiple failed attempts."));

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!signInResult.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));

            var roles = await _userManager.GetRolesAsync(user);
            var Role = roles.FirstOrDefault();

            string profileImage = string.IsNullOrEmpty(user.profileImage)
                                ? string.Empty
                                : $"{_configuration["BaseApiUrl"]}{user.profileImage}";
            var response = new
            {

                user = new
                {
                    token = await _tokenServices.CreateToken(user, _userManager),
                    displayName = user.DisplayName,
                    email = user.Email,
                    role = Role ,
                    profileImage = profileImage,
                    profession =user.profession,
                    bio = user.bio
                }
            };
            return Ok(response/*new GetCurrentUserdto(user.DisplayName, loginDto.Email, await _tokenServices.CreateToken(user, _userManager) , role)*/);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new[] { "This email already exists" } });

            if (!await _roleManager.RoleExistsAsync(registerDto.role))
            {
                return BadRequest(new ApiResponse(400, "Invalid role specified"));
            }

            var user = new User()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber ?? string.Empty // Ensure it's not null
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(user, registerDto.role);

            if (!roleAssignmentResult.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to assign role"));

            var roles = await _userManager.GetRolesAsync(user);
            var Role = roles.FirstOrDefault();

            string profileImage = string.IsNullOrEmpty(user.profileImage)
                                ? string.Empty
                                : $"{_configuration["BaseApiUrl"]}{user.profileImage}";

            var response = new
            {
                user = new
                {
                    token = await _tokenServices.CreateToken(user, _userManager),
                    displayName = user.DisplayName,
                    email = user.Email,
                    role = Role,
                    profileImage = profileImage,
                    profession = user.profession,
                    bio = user.bio
                }
            };
            return Ok(response /*new GetCurrentUserdto(registerDto.DisplayName, registerDto.Email, await _tokenServices.CreateToken(user, _userManager), role)*/);
        }

        [HttpGet("CheckEmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            var userExists = await _userManager.FindByEmailAsync(Email) != null;
            return Ok(!userExists);
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null) return Unauthorized(new ApiResponse(401));

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(); // Get the first role (assuming user has one role)

            return Ok(new GetCurrentUserdto(user.DisplayName, user.Email, await _tokenServices.CreateToken(user, _userManager), role));
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "User logged out successfully." });
        }

        [Authorize(Roles = "Admin")] 
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to delete user"));

            return Ok(new ApiResponse(200, "User deleted successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUserByEmail/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to delete user"));

            return Ok(new ApiResponse(200, "User deleted successfully"));
        }

        [Authorize]
        [HttpPut("UploadProfilePhoto")]
        public async Task<ActionResult> UpdateProfile([FromForm] UpdateProfileDto updateProfileDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new ApiResponse(401, "User email not found in token."));

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found."));

            // Validate and save profile image if provided
            if (updateProfileDto.profileImage != null)
            {
                try
                {
                    user.profileImage = await _imageService.SaveImageAsync(updateProfileDto.profileImage, "Users");
                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiResponse(400, $"Image upload failed: {ex.Message}"));
                }
            }

            user.DisplayName = updateProfileDto.DisplayName;
            user.bio = updateProfileDto.bio;
            user.profession = updateProfileDto.profession;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to update user profile."));

            string profileImage = string.IsNullOrEmpty(user.profileImage)
                                  ? string.Empty
                                  : $"{_configuration["BaseApiUrl"]}{user.profileImage}";
            return Ok(new
            {
                message = "Profile updated successfully",
                displayName = user.DisplayName,
                profileImage = profileImage,
                profession = user.profession,
                bio = user.bio
            });
        }

        [Authorize]
        [HttpGet("GetUserRole")]
        public async Task<ActionResult<string>> GetUserRole()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse(401, "User email not found in token."));
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found."));
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            return Ok(role);
        }

        [Authorize]
        [HttpGet("AddUserRole")]
        public async Task<ActionResult> AddUserRole( string role)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse(401, "User email not found in token."));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found."));

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to add role to user."));

            return Ok(new ApiResponse(200, "Role added successfully."));
        }


        //[HttpPost("RefreshToken")]
        //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        //{
        //    if (request is null || string.IsNullOrEmpty(request.RefreshToken))
        //        return BadRequest(new ApiResponse(400, "Invalid refresh token request"));

        //    var user = await _userManager.Users
        //        .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

        //    if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        //        return Unauthorized(new ApiResponse(401, "Invalid or expired refresh token"));

        //    // Generate new access token
        //    var newToken = await _tokenServices.CreateToken(user, _userManager);

        //    // Optionally, generate a new refresh token and update the user
        //    user.RefreshToken = _tokenServices.GenerateRefreshToken();
        //    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set new expiry

        //    await _userManager.UpdateAsync(user);

        //    return Ok(new AuthResponse
        //    {
        //        Token = newToken,
        //        RefreshToken = user.RefreshToken
        //    });
        //}


        //[Authorize]
        //[HttpPost("UploadProfilePhoto")]
        //public async Task<ActionResult> UploadProfilePhoto( IFormFile image)
        //{
        //    if (image == null || image.Length == 0)
        //        return BadRequest(new ApiResponse(400, "No image uploaded"));

        //    var userEmail = User.FindFirstValue(ClaimTypes.Email);
        //    var user = await _userManager.FindByEmailAsync(userEmail);
        //    if (user == null)
        //        return Unauthorized(new ApiResponse(401, "User not found"));

        //    // Save image and update user profile
        //    string imagePath = await _imageService.SaveImageAsync(image, "Users");
        //    user.profilePhotoesPath = imagePath;  // ✅ Store the image path in the user profile

        //    // Save changes to the database
        //    var result = await _userManager.UpdateAsync(user);
        //    if (!result.Succeeded)
        //        return StatusCode(500, new ApiResponse(500, "Failed to update profile photo"));

        //    return Ok(new ApiResponse(200, "Profile photo uploaded successfully"));
        //}



    }
}
