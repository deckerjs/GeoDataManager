using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using GeoDataModels.Models;
using System.Linq;
using GeoStoreAPI.Models;
using CoordinateDataModels;

namespace GeoStoreAPI.Repositories
{
    public class CoordinateDataRepository : ICoordinateDataRepository
    {
        private readonly ICoordinateDataAccess _dataAccess;
        private readonly IUserDataPermissionRepository _dataPermissionRepository;

        public CoordinateDataRepository(ICoordinateDataAccess dataAccess,
            IUserDataPermissionRepository dataPermissionRepository)
        {
            _dataAccess = dataAccess;
            _dataPermissionRepository = dataPermissionRepository;
        }

        public string Create(CoordinateData incomingData, string userID)
        {
            var createData = new CoordinateData()
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

        public IEnumerable<CoordinateData> GetAll(string userID, Func<CoordinateData, bool> filter)
        {
            Func<CoordinateData, bool> userFilter = (x) => x.UserID == userID;
            Func<CoordinateData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
            return _dataAccess.GetAll(combinedFilter);
        }

        public IEnumerable<CoordinateData> GetShared(string userID, Func<CoordinateData, bool> filter)
        {
            var data = new List<CoordinateData>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, x => true);

            foreach (var grant in grants)
            {
                Func<CoordinateData, bool> userFilter = (x) => x.UserID == grant.OwnerUserID;
                Func<CoordinateData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
                data.AddRange(_dataAccess.GetAll(combinedFilter));
            }

            return data;
        }

        public CoordinateData GetSingle(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        public void Update(string id, CoordinateData incomingData, string userID)
        {
            var existingData = _dataAccess.Get(id);
            if (existingData.UserID == userID)
            {
                var updateData = new CoordinateData()
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

        

        //private IEnumerable<CoordinateData> GetMockData()
        //{
        //    var testData = new List<CoordinateData>();
        //    var data1 = new CoordinateData()
        //    {
        //        UserID = "testuser1234",
        //        ID = Guid.NewGuid().ToString(),
        //        DateCreated = DateTime.Now,
        //        Description = "test description",
        //        Tags = { "some tag", "some other tag", "tag 3" },
        //        DateModified = DateTime.Now,
        //        Data = GetSampleFeatureCollection()
        //    };
        //    testData.Add(data1);
        //    return testData;
        //}

        

    }
}