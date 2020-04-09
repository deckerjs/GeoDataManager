using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using GeoDataModels.Models;

namespace GeoStoreAPI.Repositories
{
    public class GeoDataRepository : IGeoDataRepository
    {
        private readonly IGeoDataAccess _dataAccess;
        private readonly IUserDataPermissionRepository _dataPermissionRepository;

        public GeoDataRepository(IGeoDataAccess dataAccess,
            IUserDataPermissionRepository dataPermissionRepository)
        {
            _dataAccess = dataAccess;
            _dataPermissionRepository = dataPermissionRepository;
        }

        public string Create(GeoData incomingData, string userID)
        {
            var createData = new GeoData()
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Description = incomingData.Description,
                Tags = incomingData.Tags,
                Data = incomingData.Data
            };

            _dataAccess.Create(createData);
            return createData.ID;
        }

        public void Delete(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                _dataAccess.Delete(id);
            }
        }

        public IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter)
        {
            Func<GeoData, bool> userFilter = (x) => x.UserID == userID;
            Func<GeoData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
            return _dataAccess.GetAll(combinedFilter);
        }

        public IEnumerable<GeoData> GetShared(string userID, Func<GeoData, bool> filter)
        {
            var data = new List<GeoData>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, x => true);

            foreach (var grant in grants)
            {
                Func<GeoData, bool> userFilter = (x) => x.UserID == grant.OwnerUserID;
                Func<GeoData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
                data.AddRange(_dataAccess.GetAll(combinedFilter));
            }

            return data;
        }

        public GeoData GetSingle(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        public void Update(string id, GeoData incomingData, string userID)
        {
            var existingData = _dataAccess.Get(id);
            if (existingData.UserID == userID)
            {
                var updateData = new GeoData()
                {
                    ID = existingData.ID,
                    UserID = existingData.UserID,
                    DateCreated = existingData.DateCreated,
                    DateModified = DateTime.Now,
                    Description = incomingData.Description,
                    Tags = incomingData.Tags,
                    Data = incomingData.Data
                };

                _dataAccess.Update(id, updateData);
            }
        }





        //todo: there has to be a better way to append data, this seems wrong
        public void AppendFeatureCollection(string id, FeatureCollection featureCollection)
        {
            var features = new List<Feature>();
            foreach (var item in featureCollection.Features)
            {
                features.Add((Feature)item);
            }

            var existingGeoData = _dataAccess.Get(id);

            foreach (var item in existingGeoData.Data.Features)
            {
                features.Add((Feature)item);
            }

            var appendedData = new FeatureCollection(features);

            existingGeoData.Data = appendedData;
            existingGeoData.DateModified = DateTime.Now;

            _dataAccess.Update(existingGeoData.ID, existingGeoData);
        }


        public FeatureCollection GetCoordinatesFeatureCollection(IEnumerable<Position> coords)
        {
            var geom = new MultiPoint(coords);
            var feature = new Feature(geom);
            var features = new List<Feature>();

            features.Add(feature);
            return new FeatureCollection(features);            
        }

        private IEnumerable<GeoData> GetMockData()
        {
            var testData = new List<GeoData>();
            var data1 = new GeoData()
            {
                UserID = "testuser1234",
                ID = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                Description = "test description",
                Tags = { "some tag", "some other tag", "tag 3" },
                DateModified = DateTime.Now,
                Data = GetSampleFeatureCollection()
            };
            testData.Add(data1);
            return testData;
        }

        private FeatureCollection GetSampleFeatureCollection()
        {
            var coords = new List<Position>(){
                 new Position(-88.095398000000003, 42.918624000000001, 252.93600000000001),
                 new Position(-88.095395999999994, 42.918604000000002, 245.953),
                 new Position(-88.095110000000005, 42.918570000000003, 241.78899999999999)
             };

            var geom1 = new MultiPoint(coords);

            var props = new Dictionary<string, object>();
            props["Name"] = "prop 1 name";

            var feature1 = new Feature(geom1, props);

            var features = new List<Feature>();
            features.Add(feature1);

            var data = new FeatureCollection(features);

            return data;
        }

    }
}