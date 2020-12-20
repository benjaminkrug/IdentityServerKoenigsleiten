namespace IdentityServer
{
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Models;
    using System.Collections.Generic;

    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.grandma"
                    }
                }
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("ApiOne"),
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256())},

                    AllowedGrantTypes = GrantTypes.Code,

                    RequirePkce = true,
                    // local api => localhost:44335
                    // azure url => koenigsleitenapi.azurewebsites.net
                    RedirectUris = { "https://koenigsleitenapi.azurewebsites.net/signin-oidc" },
                    PostLogoutRedirectUris = { "https://koenigsleitenapi.azurewebsites.net/Home/Index" },

                    AllowedScopes = {
                        "ApiOne",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope"
                    },

                    // puts all the claims in the id token
                    // AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,

                    RequireConsent = false

                },
                new Client
                {
                    ClientId = "client_id_js",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost:8080/Home/SignIn" },
                    PostLogoutRedirectUris = { "http://localhost:8080/Home/Index" },
                    AllowedCorsOrigins = { "http://localhost:8080" },

                    AllowedScopes = {
                        "ApiOne",
                        IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                    },

                    AccessTokenLifetime = 30,

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false
                }
            };



    }
}
