using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using GeoStoreAPI.Services;
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
            var filters = new List<Expression<Func<AppUserRoles, bool>>>();
            filters.Add(FilterExpressionUtilities.GetEqExpressionForProperty<AppUserRoles>("UserID", userID));


            var userRoles = _dataAccess.GetAll(filters);
            if(userRoles!=null)return userRoles.FirstOrDefault();
            return null;
        }

        public void CreateUserRoles(AppUserRoles userRoles)
        {
            _dataAccess.Create(userRoles);
        }

        public void UpdateUserRoles(string id, AppUserRoles userRoles)
        {
            _dataAccess.Update(id, userRoles);
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
