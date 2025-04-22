using Waseet.System.Services.Domain.Common;

namespace Waseet.System.Services.Domain.Models
{
    public class ContactUs : BaseEntity
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
