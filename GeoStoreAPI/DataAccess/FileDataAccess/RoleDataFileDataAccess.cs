using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.DataAccess.FileDataAccess
{
    public class RoleDataFileDataAccess : IRoleDataAccess
    {
        private readonly IFileDataAccess<AppRole> _fileDataAccess;
        public const string ROLE_DATA = "RoleData";

        public RoleDataFileDataAccess(IFileDataAccess<AppRole> fileDataAccess)
        {
            _fileDataAccess = fileDataAccess;
        }

        public void Create(AppRole roleData)
        {            
            _fileDataAccess.CreateItem(ROLE_DATA, roleData.RoleID, roleData);
        }

        public void Delete(string id)
        {
            var data = _fileDataAccess.GetItem(ROLE_DATA, id);
            if (data!=null)
            {
                _fileDataAccess.DeleteItem(ROLE_DATA, id);
            }
        }

        public AppRole Get(string roleID)
        {
            var data = _fileDataAccess.GetItem(ROLE_DATA, roleID);
            if (data!=null)
            {
                return data;
            }
            return null;
        }

        public IEnumerable<AppRole> GetAll(IEnumerable<Expression<Func<AppRole, bool>>> filter)
        {   
            return _fileDataAccess.GetAllItems(ROLE_DATA, filter);
        }

        public AppRole Getsingle(string id)
        {
            return _fileDataAccess.GetItem(ROLE_DATA, id.ToString());
        }

        public void Update(string id, AppRole roleData)
        {            
            _fileDataAccess.SaveItem(ROLE_DATA,id,roleData);
        }

    }
}