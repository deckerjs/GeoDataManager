using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using GeoStoreAPI.Models;
using GeoStoreAPI.DataAccess;

namespace GeoStoreAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly AppOptions _options;

        public UserRepository(IUserDataAccess dataAccess, AppOptions options)
        {
            _dataAccess = dataAccess;
            _options = options;
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

        public void CreateUser(string userID, AppUser user)
        {
            _dataAccess.Create(user, userID);
        }

    }
}
