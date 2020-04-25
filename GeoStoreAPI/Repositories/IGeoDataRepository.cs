using System;
using System.Collections.Generic;
using GeoDataModels.Models;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IGeoDataRepository
    {
        IEnumerable<GeoJsonData> GetAll(string userID, Func<GeoJsonData, bool> filter);
        IEnumerable<GeoJsonData> GetShared(string userID, Func<GeoJsonData, bool> filter);
        GeoJsonData GetSingle(string id, string userID);
        string Create(GeoJsonData geoData, string userID);
        void Update(string id, GeoJsonData geoData, string userID);
        void Delete(string id, string userID);                
        void AppendMultiPointCollection(string id, IEnumerable<Coordinate> coords);
        List<Coordinate> GetCoordinatesFromFeatureCollection(FeatureCollection featureCollection);
    }
}