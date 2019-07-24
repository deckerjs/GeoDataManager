using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeoStoreAPI.DataAccess
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