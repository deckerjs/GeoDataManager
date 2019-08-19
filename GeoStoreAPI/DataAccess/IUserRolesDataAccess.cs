using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IUserRolesDataAccess
    {
        void Create(AppUserRoles userRoleData);
        void Delete(string id);
        AppUserRoles Get(string roleID);
        IEnumerable<AppUserRoles> GetAll(Func<AppUserRoles, bool> filter);
        AppUserRoles Getsingle(string userID);
        void Update(AppUserRoles userRoleData);
    }
}
