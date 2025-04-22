using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Domain.Models.Identity;

namespace Waseet.System.Services.Persistence.Data.DataSeeding
{
    public class UserIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "customer", "admin", "serviceProvider" };

            // Seed Roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // User definitions
            var users = new List<(string UserName, string DisplayName, string Email, string Phone, string Role)>
            {
                ("mostafa.omara", "Mostafa Nasser Omara", "mostafa20-02161@student.eelu.edu.eg", "1234567890", "admin"),
                ("ahmed.ashraf", "Ahmed ashraf Ahmed", "ahmed@example.com", "0987654321", "customer"),
                ("ola.allam", "Ola Allam Ahmed", "sara@example.com", "1122334455", "serviceProvider"),
            };

            foreach (var (UserName, DisplayName, Email, Phone, Role) in users)
            {

                if (await userManager.FindByNameAsync(UserName) == null)
                {

                    var user = new User
                    {
                        UserName = UserName,
                        DisplayName = DisplayName,
                        Email = Email,
                        PhoneNumber = Phone,
                        EmailConfirmed = true,
                    };

                    var result = await userManager.CreateAsync(user, "Pa$$w0rd");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, Role);
                    }

                }
            }
        }
    }
}
