using System;
using System.Collections.Generic;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IGeoDataAccess
    {
        void Create(GeoData geoData);
        GeoData Get(string id);
        void Delete(string id);
        IEnumerable<GeoData> GetAll(Func<GeoData, bool> filter);        
        void Update(string id, GeoData geoData);
    }
}