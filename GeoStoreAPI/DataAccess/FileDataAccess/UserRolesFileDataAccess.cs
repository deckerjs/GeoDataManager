using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{


    public class UserRolesFileDataAccess : IUserRolesDataAccess
    {

        private readonly IFileDataAccess<AppUserRoles> _fileDataAccess;
        public const string USER_ROLE_DATA = "UserRoleData";

        public UserRolesFileDataAccess(IFileDataAccess<AppUserRoles> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }

        public void Create(AppUserRoles userRoleData)
        {
            _fileDataAccess.CreateItem(USER_ROLE_DATA, userRoleData.UserID, userRoleData);
        }

        public void Delete(string id)
        {
            var data = _fileDataAccess.GetItem(USER_ROLE_DATA, id);
            if (data != null)
            {
                _fileDataAccess.DeleteItem(USER_ROLE_DATA, id);
            }
        }

        public AppUserRoles Get(string roleID)
        {
            var data = _fileDataAccess.GetItem(USER_ROLE_DATA, roleID);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public IEnumerable<AppUserRoles> GetAll(Func<AppUserRoles, bool> filter)
        {
            Func<AppUserRoles, bool> combinedFilter = (x) => filter(x);
            return _fileDataAccess.GetAllItems(USER_ROLE_DATA, combinedFilter);
        }

        public void Update(string id, AppUserRoles userRoleData)
        {
            _fileDataAccess.SaveItem(USER_ROLE_DATA, id, userRoleData);
        }
    }
}