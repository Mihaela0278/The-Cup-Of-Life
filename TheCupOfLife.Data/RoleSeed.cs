using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCupOfLife.Data.Models;

namespace TheCupOfLife.Data
{
    public class RoleSeed
    {
        public static void SeedRoles(TheCupOfLifeContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.AddRange(
                    new IdentityRole()
                    {
                        Name = Roles.USER.ToString(),
                        NormalizedName = Roles.USER.ToString().ToUpper()
                    },

                    new IdentityRole()
                    {
                        Name = Roles.ADMIN.ToString(),
                        NormalizedName = Roles.ADMIN.ToString().ToUpper()
                    }
            );

            context.SaveChanges();
        }

        public static async Task SeedDefaultAdmin(UserManager<User> userManager)
        {
            string email = "admin@gmail.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                User admin = new User()
                {
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    Email = email,
                    NormalizedEmail = email.ToUpper()
                };

                await userManager.CreateAsync(admin, "Admin@77");
                await userManager.AddToRoleAsync(admin, Roles.ADMIN.ToString());
            }

            if (userManager.GetRolesAsync(user).Result.Contains(Roles.ADMIN.ToString()))
            {
                return;
            }

            await userManager.RemoveFromRoleAsync(user, Roles.USER.ToString());
            await userManager.AddToRoleAsync(user, Roles.ADMIN.ToString());

        }
    }
}
