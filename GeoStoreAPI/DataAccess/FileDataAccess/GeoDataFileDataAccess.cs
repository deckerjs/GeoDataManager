using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess.FileDataAccess
{
    public class GeoDataFileDataAccess : IGeoDataAccess
    {
        private readonly IFileDataAccess<GeoJsonData> _fileDataAccess;

        public const string DATA_GROUP = "GeoData";

        public GeoDataFileDataAccess(IFileDataAccess<GeoJsonData> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }
        public void Create(GeoJsonData geoData)
        {
            _fileDataAccess.CreateItem(DATA_GROUP, geoData.ID, geoData);
        }

        public void Delete(string id)
        {
            _fileDataAccess.DeleteItem(DATA_GROUP, id);
        }

        public IEnumerable<GeoJsonData> GetAll(IEnumerable<Expression<Func<GeoJsonData, bool>>> filter)
        {
            return _fileDataAccess.GetAllItems(DATA_GROUP, filter);
        }

        public GeoJsonData Get(string id)
        {
            return _fileDataAccess.GetItem(DATA_GROUP, id);
        }

        public void Update(string id, GeoJsonData geoData)
        {
            _fileDataAccess.SaveItem(DATA_GROUP, id, geoData);
        }
    }
}