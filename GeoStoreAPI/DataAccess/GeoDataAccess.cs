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
        public void Create(GeoData geoData)
        {
            _fileDataAccess.CreateItem(GEODATA, geoData.ID, geoData);
        }

        public void Delete(string id)
        {
            _fileDataAccess.DeleteItem(GEODATA, id);
        }

        public IEnumerable<GeoData> GetAll(Func<GeoData, bool> filter)
        {
            return _fileDataAccess.GetAllItems(GEODATA, filter);
        }

        public GeoData Get(string id)
        {
            return _fileDataAccess.GetItem(GEODATA, id);
        }

        public void Update(string id, GeoData geoData)
        {
            _fileDataAccess.SaveItem(GEODATA, id, geoData);
        }
    }
}