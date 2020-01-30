using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.API
{
    public class Config
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("lttestproject-api", "LTTestProject API"),
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client.ro",
                    ClientName = "client.ro",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 7200,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "lttestproject-api" }
                }
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

    }
}
