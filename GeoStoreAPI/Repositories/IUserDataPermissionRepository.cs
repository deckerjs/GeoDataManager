using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Repositories
{
    public interface IUserDataPermissionRepository 
    {
        void Create( UserDataPermission data, string userID);
        void Delete(string dataPermissionId, string userID);
        UserDataPermission Get(string dataPermissionId, string userID);
        IEnumerable<UserDataPermission> GetAllGrantedToUser(Func<UserDataPermission, bool> filter, string userID);
        IEnumerable<UserDataPermission> GetAllForOwnerUser(Func<UserDataPermission, bool> filter, string userID);
        void Update(UserDataPermission data, string userID);
    }
}
