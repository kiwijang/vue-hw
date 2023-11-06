using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            // var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            var roles = new List<AppRole>
            {
                new AppRole{Name = SysRole.Admin.ToString()},
                new AppRole{Name = SysRole.User.ToString()},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            // foreach (var user in users)
            // {
            //     user.UserName = user.UserName.ToLower();
            //     user.CreateTime = DateTime.SpecifyKind(user.CreateTime, DateTimeKind.Utc);
            //     await userManager.CreateAsync(user, "Pa$$w0rd");
            //     await userManager.AddToRoleAsync(user, SysRole.User.ToString());
            // }

            var admin = new AppUser
            {
                UserName = "naomi@email.com",
                ChiName = "IAM_Admin", // UserName
                Email = "naomi@email.com",
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { SysRole.Admin.ToString() });
        }
    }
}