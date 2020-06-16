using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess
{
    public interface ICoordinateDataAccess
    {
        void Create(CoordinateData geoData);
        CoordinateData Get(string id);
        void Delete(string id);
        IEnumerable<CoordinateData> GetAll(IEnumerable<Expression<Func<CoordinateData, bool>>> filter);
        IEnumerable<CoordinateDataSummary> GetSummary(IEnumerable<Expression<Func<CoordinateData, bool>>> filter);
        void Update(string id, CoordinateData geoData);
        bool IdBelongsToUser(string id, string userId);
        void AppendToPointCollection(string id, string pcid, IEnumerable<Coordinate> coordinates);
    }
}
