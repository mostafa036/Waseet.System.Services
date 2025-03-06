using Microsoft.AspNetCore.Identity;

namespace Waseet.System.Services.Domain.Identity
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Address Address { get; set; } = null!;
        public ProfilePhotos profilePhotoes { get; set; } = null!;
    }
}
