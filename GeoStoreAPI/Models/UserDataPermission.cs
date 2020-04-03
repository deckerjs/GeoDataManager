using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Models
{
    public class UserDataPermission
    {
        public string ID { get; set; }
        public string OwnerUserID { get; set; }
        public string AllowedUserID { get; set; }
        public string ResourceName { get; set; }
        public bool Read { get; set; } = true;
        public bool Write { get; set; }
    }
}
