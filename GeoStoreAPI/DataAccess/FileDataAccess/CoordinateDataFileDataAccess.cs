using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoordinateDataModels;
using GeoDataModels.Models;

namespace GeoStoreAPI.DataAccess.FileDataAccess
{
    public class CoordinateDataFileDataAccess : ICoordinateDataAccess
    {
        private readonly IFileDataAccess<CoordinateData> _fileDataAccess;

        public const string DATA_GROUP = "CoordinateData";

        public CoordinateDataFileDataAccess(IFileDataAccess<CoordinateData> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }
        public void Create(CoordinateData geoData)
        {
            _fileDataAccess.CreateItem(DATA_GROUP, geoData.ID, geoData);
        }

        public void Delete(string id)
        {
            _fileDataAccess.DeleteItem(DATA_GROUP, id);
        }

        public IEnumerable<CoordinateData> GetAll(IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {
            return _fileDataAccess.GetAllItems(DATA_GROUP, filter);
        }

        public CoordinateData Get(string id)
        {
            return _fileDataAccess.GetItem(DATA_GROUP, id);
        }

        public void Update(string id, CoordinateData geoData)
        {
            _fileDataAccess.SaveItem(DATA_GROUP, id, geoData);
        }

        public IEnumerable<CoordinateDataInfo> GetSummary(IEnumerable<Expression<Func<CoordinateDataInfo, bool>>> filter)
        {
            throw new NotImplementedException();
        }
    }
}