﻿using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Repositories
{
    public class UserDataPermissionRepository : IUserDataPermissionRepository
    {
        private readonly IUserDataPermissionDataAccess _dataAccess;

        //todo: refactor this class to be generic, and replace other similar classes with it

        public UserDataPermissionRepository(IUserDataPermissionDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public void Create(UserDataPermission data, string userID)
        {
            var existing = GetAllGrantedToUser(data.AllowedUserID, x => x.OwnerUserID == userID);
            if(existing==null || !existing.Any())
            {
                data.OwnerUserID = userID;
                data.ID = Guid.NewGuid().ToString();
                _dataAccess.Create(data);
            }
        }

        public void Delete(string dataPermissionId, string userID)
        {
            var data = _dataAccess.Get(dataPermissionId);
            if (data != null && data.OwnerUserID == userID)
            {
                _dataAccess.Delete(dataPermissionId);
            }

        }

        public UserDataPermission Get(string dataPermissionId, string userID)
        {
            var data = _dataAccess.Get(dataPermissionId);
            if (data != null && data.OwnerUserID == userID)
            {
                return data;
            }
            return null;
        }

        public IEnumerable<UserDataPermission> GetAllGrantedToUser(string userID, Func<UserDataPermission, bool> filter )
        {
            Func<UserDataPermission, bool> userFilter = (x) => x.AllowedUserID == userID;
            Func<UserDataPermission, bool> combinedFilter = (x) => filter(x) && userFilter(x);
            return _dataAccess.GetAll(combinedFilter);
        }

        public IEnumerable<UserDataPermission> GetAllForOwnerUser(string userID, Func<UserDataPermission, bool> filter)
        {
            Func<UserDataPermission, bool> userFilter = (x) => x.OwnerUserID == userID;
            Func<UserDataPermission, bool> combinedFilter = (x) => filter(x) && userFilter(x);
            return _dataAccess.GetAll(combinedFilter);
        }

        public void Update(string id, string userID, UserDataPermission data)
        {
            data.ID = id;
            data.OwnerUserID = userID;
            _dataAccess.Update(id, data);
        }
    }
}
