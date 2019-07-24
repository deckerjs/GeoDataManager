using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace GeoStoreAPI.Models
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "100000", Username = "user1", Password = "password1", 
                Claims = 
                {
                    new Claim(JwtClaimTypes.Name, "user1"),
                    new Claim(JwtClaimTypes.GivenName, "user1"),
                    new Claim(JwtClaimTypes.FamilyName, "just user1"),
                    new Claim(JwtClaimTypes.Email, "user1@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://user1.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'there', 'locality': 'here', 'postal_code': 12345, 'country': 'mine' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser{SubjectId = "100001", Username = "user2", Password = "password2",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "user2"),
                    new Claim(JwtClaimTypes.GivenName, "user2"),
                    new Claim(JwtClaimTypes.FamilyName, "just user2"),
                    new Claim(JwtClaimTypes.Email, "user2@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://user2.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'there', 'locality': 'here', 'postal_code': 12345, 'country': 'mine' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            }

        };
    }
}