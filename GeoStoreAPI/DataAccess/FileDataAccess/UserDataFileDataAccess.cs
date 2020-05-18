using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess.FileDataAccess
{
    public class UserDataFileDataAccess : IUserDataAccess
    {
        private readonly IFileDataAccess<AppUser> _fileDataAccess;

        public const string USER_DATA = "UserData";

        public UserDataFileDataAccess(IFileDataAccess<AppUser> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }
        public void Create(AppUser userData)
        {            
            _fileDataAccess.CreateItem(USER_DATA, userData.ID.ToString(), userData);
        }

        public void Delete(string userID)
        {
            var data = _fileDataAccess.GetItem(USER_DATA, userID);
            if (data!=null)
            {
                _fileDataAccess.DeleteItem(USER_DATA, userID);
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

        public IEnumerable<AppUser> GetAll(IEnumerable<Expression<Func<AppUser, bool>>> filter)
        {            
            return _fileDataAccess.GetAllItems(USER_DATA, filter);
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

        public void Update(string userID, AppUser userData)
        {
            userData.ID = userID;
            _fileDataAccess.SaveItem(USER_DATA,userID,userData);
        }
    }
}