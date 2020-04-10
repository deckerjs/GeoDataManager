using System.Collections.Generic;
using System.Linq;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using Microsoft.Extensions.Options;

namespace GeoStoreAPI.Repositories
{

    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly IUserRolesDataAccess _dataAccess;        
        private readonly AppOptions _options;

        public UserRolesRepository(IUserRolesDataAccess dataAccess, AppOptions options)
        {
            _dataAccess = dataAccess;
            _options = options;
        }

        public AppUserRoles GetUserRoles(string userID)
        {
            var userRole = _dataAccess.GetAll(r => r.UserID == userID);
            return userRole.FirstOrDefault();
        }

        public void CreateUserRoles(AppUserRoles userRoles)
        {
            _dataAccess.Create(userRoles);
        }

        public void UpdateUserRoles(AppUserRoles userRoles)
        {
            _dataAccess.Update(userRoles);
        }

        public void RemoveUserRoles(string userID)
        {
            if (GetUserRoles(userID) != null)
            {
                _dataAccess.Delete(userID);
            }
        }

    }
}
