using IdentityServer4.Models;
using IdentityServer4.Services;
using LTTestProject.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTTestProject.Security
{
    public class ProfileService : IProfileService
    {
        private readonly LTTestProjectIdentityDbContext _identityContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ProfileService> _logger;

        //services
        public ProfileService(LTTestProjectIdentityDbContext identityContext, UserManager<IdentityUser> userManager, IServiceProvider serviceProvider, ILogger<ProfileService> logger)
        {
            _identityContext = identityContext;
            _userManager = userManager;
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _logger = logger;
        }

        //Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = await _userManager.FindByEmailAsync(context.Subject.Identity.Name);

                    if (user != null)
                    {
                        var claims = ResourceOwnerPasswordValidator.GetUserClaims(user);

                        //set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    //Guid resultId;
                    //if (!string.IsNullOrEmpty(userId?.Value) && Guid.TryParse(userId.Value, out resultId))
                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                        //get user from db (find user by user id)
                        var user = await _userManager.FindByIdAsync(userId.Value);

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = ResourceOwnerPasswordValidator.GetUserClaims(user);

                            //context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                            context.IssuedClaims = claims.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //log your error
            }
        }

        //check if user account is active.
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                //Guid resultId;
                //if (!string.IsNullOrEmpty(userId?.Value) && Guid.TryParse(userId.Value, out resultId))
                if (!string.IsNullOrEmpty(userId?.Value))
                {
                    var user = await _userManager.FindByIdAsync(userId.Value);

                    if (user != null)
                    {
                        context.IsActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //handle error logging
            }
        }
    }
}
