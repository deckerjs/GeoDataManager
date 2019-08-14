using System.Collections.Generic;

namespace GeoStoreAPI.Models
{
    public class AppUserRoles
    {
        public AppUserRoles()
        {
            RoleIDs = new List<string>();
        }

        string AppUserID { get; set; }
        List<string> RoleIDs { get; set; }
    }
}