using GeoStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess
{
    public class UserDataPermissionFileDataAccess : IUserDataPermissionDataAccess
    {
        public const string USER_DATA_PERMISSION = "UserDataPermission";
        private readonly IFileDataAccess<UserDataPermission> _fileDataAccess;

        public UserDataPermissionFileDataAccess(IFileDataAccess<UserDataPermission> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }

        public void Create(UserDataPermission data)
        {
            _fileDataAccess.CreateItem(USER_DATA_PERMISSION, data.ID, data);
        }

        public void Delete(string id)
        {
            var data = _fileDataAccess.GetItem(USER_DATA_PERMISSION, id);
            if (data != null)
            {
                _fileDataAccess.DeleteItem(USER_DATA_PERMISSION, id);
            }
        }

        public UserDataPermission Get(string id)
        {
            var data = _fileDataAccess.GetItem(USER_DATA_PERMISSION, id);
            if (data != null && data.ID == id)
            {
                return data;
            }
            return null;
        }

        public IEnumerable<UserDataPermission> GetAll(Func<UserDataPermission, bool> filter)
        {
            Func<UserDataPermission, bool> combinedFilter = (x) => filter(x);
            return _fileDataAccess.GetAllItems(USER_DATA_PERMISSION, combinedFilter);
        }

        public void Update(string id, UserDataPermission data)
        {
            _fileDataAccess.SaveItem(USER_DATA_PERMISSION, id, data);
        }
    }
}
