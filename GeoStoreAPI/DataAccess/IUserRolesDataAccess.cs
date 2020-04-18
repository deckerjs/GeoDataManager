using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IUserRolesDataAccess
    {
        void Create(AppUserRoles userRoleData);
        void Delete(string id);
        AppUserRoles Get(string id);
        IEnumerable<AppUserRoles> GetAll(Func<AppUserRoles, bool> filter);
        void Update(string id, AppUserRoles userRoleData);
    }
}
