using CoordinateDataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Repositories
{
    public interface ICoordinateDataRepository
    {
        IEnumerable<CoordinateData> GetAll(string userID, Func<CoordinateData, bool> filter);
        IEnumerable<CoordinateData> GetShared(string userID, Func<CoordinateData, bool> filter);
        CoordinateData GetSingle(string id, string userID);
        string Create(CoordinateData geoData, string userID);
        void Update(string id, CoordinateData geoData, string userID);
        void Delete(string id, string userID);

        void AddPointCollection(string id, IEnumerable<Coordinate> coordinates, string userID);
        void AppendToPointCollection(string id, string pcid, IEnumerable<Coordinate> coordinates, string userID);        
    }
}
