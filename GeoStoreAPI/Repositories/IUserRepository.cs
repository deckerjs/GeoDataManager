using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        IEnumerable<AppUser> GetAllUsers(IEnumerable<Expression<Func<AppUser, bool>>> filter);

        AppUser GetUser(string userID);

        AppUser FindByUsername(string username);
        void CreateUser(string userID, AppUser user);
        void CreateUser(AppUser user);
        void UpdateUser(AppUser user);
        void DeleteUser(string userID);
    }
}
