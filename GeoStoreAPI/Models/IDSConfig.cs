using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace GeoStoreAPI.Models
{
    public static class IDSConfig
    {


        public static IEnumerable<IdentityResource> GetIdentityResources()
        {

            var resources = new IdentityResource[]
                        {
                new IdentityResources.OpenId(){UserClaims= new []{"sub"}},
                new IdentityResources.Profile(),
                        };

            return resources;
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            //http://docs.identityserver.io/en/latest/topics/add_apis.html

            return new ApiResource[]
            {
                new ApiResource("geomgrapi", "GeoData Manager API")

            };

            //new ApiResource("IdentityServerApi")
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client

                //http://whatever.url/callback
                new Client
                {
                    ClientId = "geomgrui",
                    ClientName = "Geo Manager UI",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("xsdf8345hfsdfkjh8uerlkj3342342".Sha256()) },
                    AllowedScopes = {  "openid", "profile", "geomgrapi" },
                    AllowOfflineAccess=true
                },

                // MVC client using hybrid flow
                // new Client
                // {
                //     ClientId = "mvc",
                //     ClientName = "MVC Client",

                //     AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                //     ClientSecrets = { new Secret("kdkdkdkfkjer845hsdfkjh998sdf".Sha256()) },

                //     RedirectUris = { "http://localhost:5001/signin-oidc" },
                //     FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                //     PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                //     AllowOfflineAccess = true,
                //     AllowedScopes = { "openid", "profile", "api1" }
                // },

                // SPA client using implicit flow
                // new Client
                // {
                //     ClientId = "spa",
                //     ClientName = "SPA Client",
                //     ClientUri = "http://identityserver.io",

                //     AllowedGrantTypes = GrantTypes.Implicit,
                //     AllowAccessTokensViaBrowser = true,

                //     RedirectUris =
                //     {
                //         "http://localhost:8100/index.html",
                //         "http://localhost:8100/callback.html",
                //         "http://localhost:8100/silent.html",
                //         "http://localhost:8100/popup.html",
                //     },

                //     PostLogoutRedirectUris = { "http://localhost:8100/index.html" },
                //     AllowedCorsOrigins = { "http://localhost:8100" },

                //     AllowedScopes = { "openid", "profile", "api1" }
                // }
            };
        }
    }
}