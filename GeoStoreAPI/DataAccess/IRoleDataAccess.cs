using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IRoleDataAccess
    {
        void Create(AppRole role, string roleID);
        IEnumerable<AppRole> GetAll(Func<AppRole, bool> filter);                
        AppRole Get(string roleID);
        void Update(AppRole roleData, string roleID);
        void Delete(string id);        
    }

}