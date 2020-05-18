using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public interface IRoleDataAccess
    {
        void Create(AppRole role);
        IEnumerable<AppRole> GetAll(IEnumerable<Expression<Func<AppRole, bool>>> filter);                
        AppRole Get(string roleID);
        void Update(string id, AppRole roleData);
        void Delete(string id);        
    }

}