using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using GeoStoreAPI.Models;
using GeoStoreAPI.DataAccess;

namespace GeoStoreAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {

        private readonly IRoleDataAccess _dataAccess;
        private readonly AppOptions _options;

        public RoleRepository(IRoleDataAccess dataAccess, AppOptions options)
        {
            _dataAccess = dataAccess;
            _options = options;
        }

        public AppRole GetRole(string roleId)
        {
            var roles = _dataAccess.GetAll(r => r.RoleID == roleId);
            if(roles!=null) return roles.FirstOrDefault();
            return null;
        }

        public void CreateRole(AppRole role)
        {
            _dataAccess.Create(role);
        }

        public void UpdateRole(string roleID, AppRole role){
            _dataAccess.Update(roleID, role);
        }

        public void RemoveRole(string roleID)
        {
            _dataAccess.Delete(roleID);
        }

    }
}
