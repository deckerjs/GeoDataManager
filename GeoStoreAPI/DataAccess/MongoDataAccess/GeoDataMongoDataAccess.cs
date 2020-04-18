using GeoDataModels.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class GeoDataMongoDataAccess : MongoDataAccessBase<GeoData>, IGeoDataAccess
    {
        public GeoDataMongoDataAccess(MongoDataContext dataContext, ILogger<GeoDataMongoDataAccess> logger)
            : base(dataContext, logger) { }

        public override string CollectionName { get; set; } = MongoSettings.CollectionNames.GEODATA;
        public override string KeyIdName { get; set; } = nameof(GeoData.ID);
    }

}
