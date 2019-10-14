using System;
using System.Collections.Generic;
using GeoDataModels.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IGeoDataRepository
    {
        IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter);
        GeoData GetSingle(Guid id, string userID);
        void Create(GeoData geoData, string userID);
        void Update(Guid id, GeoData geoData, string userID);
        void Delete(Guid id, string userID);
    }
}