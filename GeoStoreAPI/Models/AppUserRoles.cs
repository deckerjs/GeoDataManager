using System.Collections.Generic;

namespace GeoStoreAPI.Models
{
    public class AppUserRoles
    {
        public AppUserRoles()
        {
            RoleIDs = new List<string>();
        }

        public string UserID { get; set; }
        public List<string> RoleIDs { get; set; }
    }
}