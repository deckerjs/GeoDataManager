using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<CoordinateDataSummary> GetSummary(IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {
            var data = _fileDataAccess.GetAllItems(DATA_GROUP, new List<Expression<Func<CoordinateData, bool>>>());
            return data.Where(y => filter.All(z => z.Compile()(y) == true)).Select(x=> new CoordinateDataSummary() 
            { 
                DataItemCount = x.Data.Count,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Description = x.Description,
                ID = x.ID,
                Tags = x.Tags,
                UserID = x.UserID
            });           

        }

        public bool IdBelongsToUser(string id, string userId)
        {
            var filter = new List<Expression<Func<CoordinateData, bool>>>();
            filter.Add(x => x.ID == id && x.UserID == userId);
            return _fileDataAccess.GetAllItems(DATA_GROUP, filter).Any();
        }

        public void AppendToPointCollection(string id, string pcid, IEnumerable<CoordinateDataModels.Coordinate> coordinates)
        {
            var item = Get(id);
            var pc = item.Data.Where(x => x.ID == pcid).FirstOrDefault();
            if(pc != null)
            {
                pc.Coordinates.AddRange(coordinates);
                Update(id, item);
            }

        }
    }
}