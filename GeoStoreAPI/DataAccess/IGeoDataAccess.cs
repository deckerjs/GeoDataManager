using System;
using System.Collections.Generic;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IGeoDataAccess
    {
        void Create(GeoJsonData geoData);
        GeoJsonData Get(string id);
        void Delete(string id);
        IEnumerable<GeoJsonData> GetAll(Func<GeoJsonData, bool> filter);        
        void Update(string id, GeoJsonData geoData);
    }
}