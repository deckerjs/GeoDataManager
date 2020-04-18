using GeoStoreAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class UserRolesMongoDataAccess : MongoDataAccessBase<AppUserRoles>, IUserRolesDataAccess
    {
        public UserRolesMongoDataAccess(MongoDataContext dataContext, ILogger<UserRolesMongoDataAccess> logger)
            : base(dataContext, logger) { }

        public override string CollectionName { get; set; } = MongoSettings.CollectionNames.USERROLES;
        public override string KeyIdName { get; set; } = nameof(AppUserRoles.UserID);
    }
}
