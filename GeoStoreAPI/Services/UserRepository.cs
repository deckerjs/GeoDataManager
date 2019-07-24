using System.Collections.Generic;
using System.Linq;
using System;

namespace GeoStoreAPI.Services
{
    public class UserRepository : IUserRepository
    {

        private readonly IUserDataAccess _dataAccess;

        public UserRepository(IUserDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        
        //uncomment this to init test users
        // _dataAccess.Create(_users[0],"100001");
        // _dataAccess.Create(_users[1],"100000");
        }

        // private readonly List<AppUser> _users = new List<AppUser>
        // {
        //     new AppUser{
        //         ID = "100000",
        //         UserName = "user1",
        //         Password = "password1",
        //         Email = "user1@email.com"
        //     },
        //     new AppUser{
        //         ID = "100001",
        //         UserName = "user2",
        //         Password = "password2",
        //         Email = "user2@email.com"
        //     }
        // };


        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }

            return false;
        }

        public AppUser FindBySubjectId(string subjectId)
        {
            var users = _dataAccess.GetAll(x=>x.ID==subjectId);
            return users.FirstOrDefault();
        }

        public AppUser FindByUsername(string username)
        {
            var users = _dataAccess.GetAll(x=>x.UserName==username);
            return users.FirstOrDefault();
        }
    }
}
