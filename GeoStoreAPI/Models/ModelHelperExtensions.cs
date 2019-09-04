using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GeoStoreAPI.Models
{
    public static class AppUserExtensions
    {
        public static void UpdateWith(this AppUser existingUser, AppUser userUpdate)
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

        public static void UpdateWith(this AppUser existingUser, AppUserBase userUpdate)
        {
                existingUser.UserName = userUpdate.UserName;                
                existingUser.Email = userUpdate.Email;
        }

    }
}