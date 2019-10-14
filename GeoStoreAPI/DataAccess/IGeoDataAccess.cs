using System;
using System.Collections.Generic;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IGeoDataAccess
    {
        void Create(GeoData geoData, string userID);
        GeoData Get(Guid id, string userID);
        void Delete(Guid id, string userID);
        IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter);
        GeoData Getsingle(Guid id, string userID);
        void Update(Guid id, GeoData geoData, string userID);
    }
}