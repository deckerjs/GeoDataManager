using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IUserDataAccess
    {
        void Create(AppUser userData);
        AppUser Get(string userID);
        void Delete(string userID);
        IEnumerable<AppUser> GetAll(Func<AppUser, bool> filter);
        void Update(string userID, AppUser userData);        
    }
}