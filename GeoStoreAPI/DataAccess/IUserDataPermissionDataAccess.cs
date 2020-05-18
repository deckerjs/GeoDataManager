using GeoStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess
{
    public interface IUserDataPermissionDataAccess
    {
        void Create(UserDataPermission data);
        UserDataPermission Get(string id);
        void Delete(string id);
        IEnumerable<UserDataPermission> GetAll(IEnumerable<Expression<Func<UserDataPermission, bool>>> filter);
        void Update(string id, UserDataPermission data);
    }
}
