using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Identity;

namespace Waseet.System.Services.Persistence.Data.DataSeeding
{
   public class UserIdentityDbContextSeed
   {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User()
                {
                    UserName = "Mostafa.omara",
                    DisplayName = "Mostafa Nasser Omara",
                    Email = "mostafa20-02161@student.eelu.edu.eg",
                    PhoneNumber = "1234567890",
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
   }
}