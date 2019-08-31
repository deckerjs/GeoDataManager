using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.Repositories
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        IEnumerable<AppUser> GetAllUsers(Func<AppUser,bool> filter);
        
        AppUser FindBySubjectId(string subjectId);

        AppUser FindByUsername(string username);
        void CreateUser(string userID, AppUser user);
    }
}
