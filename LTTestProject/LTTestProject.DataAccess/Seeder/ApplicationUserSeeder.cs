using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.DataAccess.Seeder
{
    public static class ApplicationUserSeeder
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            string email = "admin@admin.com";
            string password = "Admin123!";

            if (userManager.FindByEmailAsync(email).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, password).Result;
            }
        }
    }
}
