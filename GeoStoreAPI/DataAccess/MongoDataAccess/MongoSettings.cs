using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }

        public static class CollectionNames
        {
            public const string DATA_GROUP = "CoordinateData";
            public const string ROLEDATA = "RoleData";
            public const string USERDATA = "UserData";
            public const string USERDATAPERMISSION = "UserDataPermission";
            public const string USERROLES = "UserRoles";
        }

    }
}
