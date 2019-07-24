using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly IFileDataAccess<AppUser> _fileDataAccess;

        private const string USER_DATA = "UserData";

        public UserDataAccess(IFileDataAccess<AppUser> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }
        public void Create(AppUser userData, string userID)
        {
            userData.ID = userID;
            _fileDataAccess.CreateItem(USER_DATA, userData.ID.ToString(), userData);
        }

        public void Delete(Guid id, string userID)
        {
            var data = _fileDataAccess.GetItem(USER_DATA, id.ToString());
            if (data!=null && data.ID == userID)
            {
                _fileDataAccess.DeleteItem(USER_DATA, id.ToString());
            }
        }

        public AppUser Get(string userID)
        {
            var data = _fileDataAccess.GetItem(USER_DATA, userID);
            if (data!=null && data.ID == userID)
            {
                return data;
            }
            return null;
        }

        public IEnumerable<AppUser> GetAll(Func<AppUser, bool> filter)
        {            
            Func<AppUser, bool>  combinedFilter = (x) => filter(x);
            return _fileDataAccess.GetAllItems(USER_DATA, combinedFilter);
        }

        public AppUser Getsingle(Guid id, string userID)
        {
            var data = _fileDataAccess.GetItem(USER_DATA, id.ToString());
            if (data.ID == userID)
            {
                return data;
            }
            return null;
        }

        public void Update(AppUser userData, string userID)
        {
            userData.ID = userID;
            _fileDataAccess.SaveItem(USER_DATA,userID,userData);
        }
    }
}