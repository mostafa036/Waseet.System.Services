using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Domain.Identity
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string AddressType { get; private set; } = string.Empty;
        public string IsPrimary { get; private set; } = string.Empty;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
