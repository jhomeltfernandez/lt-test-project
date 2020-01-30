using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LTTestProject.API
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

        public ResourceOwnerPasswordValidator(UserManager<IdentityUser> userManager, IServiceProvider serviceProvider, ILogger<ResourceOwnerPasswordValidator> logger)
        {
            _userManager = userManager;
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _logger = logger;
        }

        //this is used to validate your user account with provided grant at /connect/token
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //get your user model from db (by username - in my case its email)
                var user = await _userManager.FindByEmailAsync(context.UserName);
                if (user != null)
                {
                    //check if password match - remember to hash password if stored as hash in db
                    bool isPasswordMatch = await _userManager.CheckPasswordAsync(user, context.Password);
                    if (isPasswordMatch)
                    {
                        //set the result
                        context.Result = new GrantValidationResult(
                                subject: user.Id.ToString(),
                                authenticationMethod: "custom",
                                claims: GetUserClaims(user)
                            );

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password!");
            }
        }

        //build claims array from user data
        public static Claim[] GetUserClaims(IdentityUser user)
        {
            //string userRole = !string.IsNullOrEmpty(user.Role) ? user.Role : string.Empty;

            return new Claim[]
            {
                    new Claim("sub", user.Id.ToString() ?? ""),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim(JwtClaimTypes.Email, user.Email  ?? "")
            };
        }
    }
}
