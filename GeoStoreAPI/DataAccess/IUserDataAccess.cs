using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IUserDataAccess
    {
        void Create(AppUser userData, string userID);
        AppUser Get(string userID);
        void Delete(Guid id, string userID);
        IEnumerable<AppUser> GetAll(Func<AppUser, bool> filter);
        void Update(AppUser userData, string userID);
    }
}