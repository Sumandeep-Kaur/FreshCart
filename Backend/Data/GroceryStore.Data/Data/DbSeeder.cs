using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace GroceryStore.Data.Data
{
    public class DbSeeder
    {
        public static async Task SeedAdminUser(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));

            var admin = new IdentityUser
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
                PhoneNumber = "9876543210"
            };

            var userInDb = await userManager.FindByEmailAsync(admin.Email);
            if(userInDb is null)
            {
                await userManager.CreateAsync(admin, "#Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    
    }
}
