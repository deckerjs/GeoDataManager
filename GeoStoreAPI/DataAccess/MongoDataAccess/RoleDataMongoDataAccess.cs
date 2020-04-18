using GeoStoreAPI.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class RoleDataMongoDataAccess : MongoDataAccessBase<AppRole>, IRoleDataAccess
    {
        public RoleDataMongoDataAccess(MongoDataContext dataContext, ILogger<RoleDataMongoDataAccess> logger)
            : base(dataContext, logger) { }

        public override string CollectionName { get; set; } = MongoSettings.CollectionNames.ROLEDATA;
        public override string KeyIdName { get; set; } = nameof(AppRole.RoleID);
    }
}
