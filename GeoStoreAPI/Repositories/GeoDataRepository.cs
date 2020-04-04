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

        public void Create(GeoData incomingData, string userID)
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
    }
}