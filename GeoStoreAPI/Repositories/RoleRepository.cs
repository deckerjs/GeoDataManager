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
        private readonly IOptionsSnapshot<AppOptions> _options;

        public RoleRepository(IRoleDataAccess dataAccess, IOptionsSnapshot<AppOptions> options)
        {
            _dataAccess = dataAccess;
            _options = options;

            if (_options.Value.GenerateDefaultUsers == true)
            {
                CreateDefaultRolesIfAbsent();
            }
        }

        public AppRole GetRole(string roleId)
        {
            var role = _dataAccess.GetAll(r => r.RoleID == roleId);
            return role.FirstOrDefault();
        }

        private void CreateDefaultRolesIfAbsent()
        {

            List<AppRole> _roles = new List<AppRole>
            {
                new AppRole{
                    RoleID = "user",
                    Description = "a user"
                },
                new AppRole{
                    RoleID = "admin",
                    Description = "an admin"
                }
            };

            _roles.ForEach(role =>
            {
                if (GetRole(role.RoleID) == null)
                {
                    CreateRole(role);
                }
            });
        }

        public void CreateRole(AppRole role)
        {
            _dataAccess.Create(role);
        }

        public void UpdateRole(AppRole role){
            _dataAccess.Update(role);
        }

        public void RemoveRole(string roleID)
        {
            _dataAccess.Delete(roleID);
        }

    }
}
