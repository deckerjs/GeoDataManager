using System.Collections.Generic;
using System.Linq;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using Microsoft.Extensions.Options;

namespace GeoStoreAPI.Repositories
{
    public class UserRolesRepository:IUserRolesRepository
    {
        private readonly IUserRolesDataAccess _dataAccess;
        private readonly IOptionsSnapshot<AppOptions> _options;

        public UserRolesRepository(IUserRolesDataAccess dataAccess, IOptionsSnapshot<AppOptions> options)
        {
            _dataAccess = dataAccess;
            _options = options;

            if (_options.Value.GenerateDefaultUsers == true)
            {
                CreateDefaultUserRolesIfAbsent();
            }
        }

        public AppUserRoles GetUserRoles(string userID)
        {
            var userRole = _dataAccess.GetAll(r => r.UserID == userID);
            return userRole.FirstOrDefault();
        }

        private void CreateDefaultUserRolesIfAbsent()
        {

            List<AppUserRoles> _userRoles = new List<AppUserRoles>
            {
                new AppUserRoles{
                    UserID = "100000",
                    RoleIDs = new List<string>() {"user"}
                },
                new AppUserRoles{
                    UserID = "100001",
                    RoleIDs = new List<string>() {"user"}
                },
                new AppUserRoles{
                    UserID = "100003",
                    RoleIDs = new List<string>() {"user", "admin"}
                }
            };

            _userRoles.ForEach(userRole =>
            {
                if (GetUserRoles(userRole.UserID) == null)
                {
                    CreateUserRoles(userRole);
                }
            });
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
            _dataAccess.Delete(userID);
        }

    }
}
