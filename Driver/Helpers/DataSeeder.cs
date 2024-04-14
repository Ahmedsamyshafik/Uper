using Driver.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Driver.Helpers
{
    public static class DataSeeder
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create an  roles if it doesn't exist
            var rolesList = new List<string>
            {
                  Constants.AdminRole, Constants.UserRole, Constants.DriverRole
            };
            foreach (var role in rolesList)
            {
                var Currenr_Role = roleManager.FindByNameAsync(role).Result; // لما يضرب ايرور هنا اقفلوا الفيجوال وافتحوه تاني
                if (Currenr_Role == null)
                {
                    Currenr_Role = new IdentityRole(role);
                    var result = roleManager.CreateAsync(Currenr_Role).Result;
                }
            }


            // Create an admin user if it doesn't exist
            var adminUser = userManager.FindByNameAsync("Admin").Result;
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01095385375",
                    Region = "Muslim",
                    EmailConfirmed = true,
                    IsActive= true,
                    Address = "Domiat" // Your Address
                };

                var result = userManager.CreateAsync(adminUser, "Alahly1907#").Result;

                // Add the admin user to the admin role
                if (result.Succeeded)
                {
                    userManager.AddToRolesAsync(adminUser, rolesList).GetAwaiter().GetResult();
                }


            }
        }
    }

}
