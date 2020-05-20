using CoordinateDataModels;
using GeoDataModels.Models;
using GeoStoreAPI.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class CoordinateDataMongoDataAccess : MongoDataAccessBase<CoordinateData>, ICoordinateDataAccess
    {
        public CoordinateDataMongoDataAccess(MongoDataContext dataContext, ILogger<CoordinateDataMongoDataAccess> logger)
            : base(dataContext, logger) { }

        public override string CollectionName { get; set; } = MongoSettings.CollectionNames.DATA_GROUP;
        public override string KeyIdName { get; set; } = nameof(CoordinateData.ID);

        public IEnumerable<CoordinateDataInfo> GetSummary(IEnumerable<Expression<Func<CoordinateDataInfo, bool>>> filter)
        {
            FilterDefinition<CoordinateDataInfo> fd = GetFilterFromExpression<CoordinateDataInfo>(filter);
            return GetCollection<CoordinateDataInfo>().Find(fd).ToList();            
        }
    }

}
