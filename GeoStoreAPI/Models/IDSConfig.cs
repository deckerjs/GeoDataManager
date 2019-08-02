using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
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
        }

        public static IEnumerable<Client> GetClients(IConfiguration Configuration)
        {
            var geoMgrSecret = Configuration.GetValue<string>("GeoMgrClientSecret");
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "geomgrui",
                    ClientName = "Geo Manager UI",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(geoMgrSecret.Sha256()) },
                    AllowedScopes = {  "openid", "profile", "geomgrapi" },
                    AllowOfflineAccess=true
                },
            };
        }
    }
}