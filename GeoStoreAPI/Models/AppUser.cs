using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GeoStoreAPI.Models
{

    public class AppUser : AppUserBase
    {
        public AppUser(AppUserBase appUserBase) : base(appUserBase){}

        public AppUser(): base(new AppUserBase())
        {
            Claims = new List<Claim>();
        }

        public string ID { get; set; }
        public bool EmailVerified { get; set; }
        public bool Disabled { get; set; }
        public string Password { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }

    public class AppUserBase
    {
        public AppUserBase(){}
        public AppUserBase(AppUserBase appUserBase)
        {
            UserName = appUserBase.UserName;            
            Email = appUserBase.Email;
        }

        public string UserName { get; set; }        
        public string Email { get; set; }
    }

}