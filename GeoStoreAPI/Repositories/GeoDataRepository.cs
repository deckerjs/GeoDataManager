using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using GeoDataModels.Models;

namespace GeoStoreAPI.Repositories
{
    public class GeoDataRepository : IGeoDataRepository
    {
        private readonly IGeoDataAccess _dataAccess;

        public GeoDataRepository(IGeoDataAccess dataAccess)
        {
            this._dataAccess = dataAccess;
        }

        public void Create(GeoData geoData, string userID)
        {            
            geoData.ID = Guid.NewGuid().ToString();
            _dataAccess.Create(geoData, userID);
        }

        public void Delete(Guid id, string userID)
        {
            _dataAccess.Delete(id, userID);
        }

        public IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter)
        {           

            return _dataAccess.GetAll(userID, filter);
        }

        public GeoData GetSingle(Guid id, string userID)
        {
            return _dataAccess.Getsingle(id, userID);
        }

        public void Update(Guid id, GeoData geoData, string userID)
        {
            geoData.UserID = userID;
            _dataAccess.Update( id, geoData, userID);
        }
    }
}