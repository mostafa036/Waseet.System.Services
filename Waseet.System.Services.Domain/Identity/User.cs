using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Domain.Identity
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CountryId { get; set; }
        public Address Address { get; set; } = null!;
        public ProfilePhotos profilePhotoes { get; set; } = null!;
    }
}
