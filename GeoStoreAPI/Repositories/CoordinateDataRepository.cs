using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using System.Linq;
using CoordinateDataModels;
using System.Linq.Expressions;
using GeoStoreAPI.Services;
using GeoStoreAPI.Models;

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

            foreach (var pc in createData.Data)
            {
                if (string.IsNullOrEmpty(pc.ID))
                {
                    pc.ID = Guid.NewGuid().ToString();
                }
            }

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

        public IEnumerable<CoordinateData> GetAll(string userID, IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {            
            var filters = filter.ToList();
            filters.Add(GetUserFilter<CoordinateData>(userID));
            return _dataAccess.GetAll(filters);
        }

        public IEnumerable<CoordinateDataSummary> GetSummary(string userID, IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {            
            var filters = filter.ToList();
            filters.Add(GetUserFilter<CoordinateData>(userID));
            return _dataAccess.GetSummary(filters);
        }

        public IEnumerable<CoordinateData> GetShared(string userID, IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {
            var data = new List<CoordinateData>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, new List<Expression<Func<UserDataPermission, bool>>>());

            foreach (var grant in grants)
            {                
                var filters = filter.ToList();
                filters.Add(GetUserFilter<CoordinateData>(grant.OwnerUserID));
                data.AddRange(_dataAccess.GetAll(filters));
            }

            return data;
        }

        public IEnumerable<CoordinateDataSummary> GetSummaryShared(string userID, IEnumerable<Expression<Func<CoordinateData, bool>>> filter)
        {
            var data = new List<CoordinateDataSummary>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, new List<Expression<Func<UserDataPermission, bool>>>());

            foreach (var grant in grants)
            {
                var filters = filter.ToList();
                filters.Add(GetUserFilter<CoordinateData>(grant.OwnerUserID));
                data.AddRange(_dataAccess.GetSummary(filters));
            }

            return data;
        }

        public CoordinateData GetSingle(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID || DataIsShared(userID, data))
            {
                return data;
            }
            return null;
        }

        public void Update(string id, CoordinateData incomingData, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                var updateData = new CoordinateData()
                {
                    ID = data.ID,
                    UserID = data.UserID,
                    DateCreated = data.DateCreated,
                    DateModified = DateTime.Now,
                    Description = incomingData.Description,
                    Tags = incomingData.Tags,
                    Data = incomingData.Data
                };
                _dataAccess.Update(id, updateData);
            }
        }

        public void AddPointCollection(string id, IEnumerable<Coordinate> coordinates, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                var newPc = new PointCollection
                {
                    ID = Guid.NewGuid().ToString(),
                    Coordinates = coordinates.ToList()
                };
                data.Data.Add(newPc);
                _dataAccess.Update(id, data);
            }
        }

        public void AppendToPointCollection(string id, string pcid, IEnumerable<Coordinate> coordinates, string userID)
        {
            if(_dataAccess.IdBelongsToUser(id, userID))
            {
                _dataAccess.AppendToPointCollection(id, pcid, coordinates);
            }
        }

        private bool DataIsShared(string userID, CoordinateData data)
        {
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, new List<Expression<Func<UserDataPermission, bool>>>());
            return grants.Where(x => x.OwnerUserID == data.UserID).Any();
        }


        private static Expression<Func<T, bool>> GetUserFilter<T>(string userID)
        {
            return FilterExpressionUtilities.GetEqExpressionForProperty<T>("UserID", userID);
        }


    }
}