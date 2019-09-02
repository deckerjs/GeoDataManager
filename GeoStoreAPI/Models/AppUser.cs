using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GeoStoreAPI.Models
{

    public class AppUser
    {
        public AppUser()
        {
            Claims = new List<Claim>();
        }

        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Disabled { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }

    public static class AppUserExtensions
    {
        public static void UpdateWith(this AppUser userUpdate, AppUser existingUser)
        {
            if (existingUser.ID == userUpdate.ID)
            {
                existingUser.UserName = userUpdate.UserName;
                existingUser.Password = userUpdate.Password;
                existingUser.Email = userUpdate.Email;
                existingUser.Disabled = userUpdate.Disabled;
                existingUser.Claims = userUpdate.Claims;
            }
            else
            {
                throw new Exception("User ID doesn't match, Not updating");
            }
        }
    }
}