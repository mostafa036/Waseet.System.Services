using Microsoft.AspNetCore.Identity;

namespace Waseet.System.Services.Domain.Models.Identity
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? profileImage { get; set; }
        public string? profession { get; set; }
        public string? bio { get; set; }
    }
}