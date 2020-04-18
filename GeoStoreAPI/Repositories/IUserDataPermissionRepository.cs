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
        IEnumerable<UserDataPermission> GetAllGrantedToUser(string userID, Func<UserDataPermission, bool> filter);
        IEnumerable<UserDataPermission> GetAllForOwnerUser(string userID, Func<UserDataPermission, bool> filter);
        void Update(string id, string userID, UserDataPermission data);
    }
}
