using GeoStoreAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class UserDataMongoDataAccess : MongoDataAccessBase<AppUser>, IUserDataAccess
    {
        public UserDataMongoDataAccess(MongoDataContext dataContext, ILogger<UserDataMongoDataAccess> logger)
            : base(dataContext, logger) { }

        public override string CollectionName { get; set; } = MongoSettings.CollectionNames.USERDATA;
        public override string KeyIdName { get; set; } = nameof(AppUser.ID);
    } 


    //{


        //public void Create(AppUser userData, string userID)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Delete(string userID)
        //{
        //    throw new NotImplementedException();
        //}

        //public AppUser Get(string userID)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<AppUser> GetAll(Func<AppUser, bool> filter)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(AppUser userData, string userID)
        //{
        //    throw new NotImplementedException();
        //}
    //}
}
