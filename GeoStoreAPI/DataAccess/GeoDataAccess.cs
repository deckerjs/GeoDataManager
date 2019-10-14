using System;
using System.Collections.Generic;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess
{
    public class GeoDataAccess : IGeoDataAccess
    {
        private readonly IFileDataAccess<GeoData> _fileDataAccess;

        public const string GEODATA = "GeoData";

        public GeoDataAccess(IFileDataAccess<GeoData> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }
        public void Create(GeoData geoData, string userID)
        {
            geoData.UserID = userID;
            _fileDataAccess.CreateItem(GEODATA, geoData.ID.ToString(), geoData);
        }

        public void Delete(Guid id, string userID)
        {
            var data = _fileDataAccess.GetItem(GEODATA, id.ToString());
            if (data!=null && data.UserID == userID)
            {
                _fileDataAccess.DeleteItem(GEODATA, id.ToString());
            }
        }

        public GeoData Get(Guid id, string userID)
        {
            var data = _fileDataAccess.GetItem(GEODATA, id.ToString());
            if (data!=null && data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        public IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter)
        {
            Func<GeoData, bool> userFilter = (x) => x.UserID == userID;
            Func<GeoData, bool>  combinedFilter = (x) => filter(x) && userFilter(x);
            return _fileDataAccess.GetAllItems(GEODATA, combinedFilter);
        }

        public GeoData Getsingle(Guid id, string userID)
        {
            var data = _fileDataAccess.GetItem(GEODATA, id.ToString());
            if (data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        public void Update(Guid id, GeoData geoData, string userID)
        {
            geoData.UserID = userID;
            _fileDataAccess.SaveItem(GEODATA,id.ToString(),geoData);
        }
    }
}