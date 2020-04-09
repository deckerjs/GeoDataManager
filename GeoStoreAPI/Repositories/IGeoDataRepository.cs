using System;
using System.Collections.Generic;
using GeoDataModels.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IGeoDataRepository
    {
        IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter);
        IEnumerable<GeoData> GetShared(string userID, Func<GeoData, bool> filter);
        GeoData GetSingle(string id, string userID);
        string Create(GeoData geoData, string userID);
        void Update(string id, GeoData geoData, string userID);
        void Delete(string id, string userID);

        FeatureCollection GetCoordinatesFeatureCollection(IEnumerable<Position> coords);
        void AppendFeatureCollection(string id, FeatureCollection featureCollection);
    }
}