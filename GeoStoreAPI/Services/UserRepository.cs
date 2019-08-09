using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;

namespace GeoStoreAPI.Services
{
    public class UserRepository : IUserRepository
    {

        private readonly IUserDataAccess _dataAccess;
        private readonly IOptionsSnapshot<AppOptions> _options;

        public UserRepository(IUserDataAccess dataAccess, IOptionsSnapshot<AppOptions> options)
        {
            _dataAccess = dataAccess;
            _options = options;

            if (_options.Value.GenerateDefaultUsers == true)
            {
                CreateDefaultUsersIfAbsent();
            }
        }

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
            var users = _dataAccess.GetAll(x => x.ID == subjectId);
            return users.FirstOrDefault();
        }

        public AppUser FindByUsername(string username)
        {
            var users = _dataAccess.GetAll(x => x.UserName == username);
            return users.FirstOrDefault();
        }

        public void CreateDefaultUsersIfAbsent()
        {
            //todo: check config settings for default user collection
            //if absent then use the ones here, also add one with an admin role

            List<AppUser> _users = new List<AppUser>
            {
                new AppUser{
                    ID = "100000",
                    UserName = "user1",
                    Password = "password1",
                    Email = "user1@email.com"
                },
                new AppUser{
                    ID = "100001",
                    UserName = "user2",
                    Password = "password2",
                    Email = "user2@email.com"
                }
            };

            if (FindBySubjectId(_users[0].ID) == null)
            {
                CreateUser(_users[0].ID, _users[0]);
            }

            if (FindBySubjectId(_users[1].ID) == null)
            {
                CreateUser(_users[1].ID, _users[1]);
            }
        }

        public void CreateUser(string userID, AppUser user)
        {
            _dataAccess.Create(user, userID);
        }



    }
}
