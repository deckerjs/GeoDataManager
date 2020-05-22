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

        public IEnumerable<CoordinateDataSummary> GetSummary(IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {
            FilterDefinition<CoordinateData> fd = GetFilterFromExpression<CoordinateData>(filter);

            var projection = Builders<CoordinateData>.Projection.Expression(x => new CoordinateDataSummary
            {
                DataItemCount = x.Data.SelectMany(x=>x.Coordinates).Count(),
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Description = x.Description,
                ID = x.ID,
                Tags = x.Tags,
                UserID = x.UserID                
            });            

            var result = GetCollection<CoordinateData>().Aggregate().Match(fd).Project(projection);

            return result.ToList();
        }
    }

}
