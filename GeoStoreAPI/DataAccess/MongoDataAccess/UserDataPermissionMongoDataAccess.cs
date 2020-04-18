using GeoStoreAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class UserDataPermissionMongoDataAccess : MongoDataAccessBase<UserDataPermission>, IUserDataPermissionDataAccess
    {
        public UserDataPermissionMongoDataAccess(MongoDataContext dataContext, ILogger<UserDataPermission> logger)
            : base(dataContext, logger) { }

        public override string CollectionName { get; set; } = MongoSettings.CollectionNames.USERDATAPERMISSION;
        public override string KeyIdName { get; set; } = nameof(UserDataPermission.ID);
    }

}
