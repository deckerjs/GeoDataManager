using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IGeoDataAccess
    {
        void Create(GeoJsonData geoData);
        GeoJsonData Get(string id);
        void Delete(string id);
        IEnumerable<GeoJsonData> GetAll(IEnumerable<Expression<Func<GeoJsonData, bool>>> filter);        
        void Update(string id, GeoJsonData geoData);
    }
}