using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using GeoStoreAPI.Models;
using GeoStoreAPI.DataAccess;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using GeoStoreAPI.Services;

namespace GeoStoreAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly AppOptions _options;
        private readonly IDataProtectionService _dataProtection;

        public UserRepository(IUserDataAccess dataAccess, AppOptions options, IDataProtectionService dataProtection)
        {
            _dataAccess = dataAccess;
            _options = options;
            _dataProtection = dataProtection;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return _dataProtection.PasswordMatchesHash(password, user.Password);
                //return user.Password.Equals(password);
            }

            return false;
        }

        public IEnumerable<AppUser> GetAllUsers(Func<AppUser, bool> filter)
        {
            return _dataAccess.GetAll(filter);
        }

        public AppUser GetUser(string subjectId)
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
            user.Password = _dataProtection.GetPasswordHash(user.Password);
            _dataAccess.Create(user, userID);
        }

        public void CreateUser(AppUser user)
        {
            string userID = Guid.NewGuid().ToString();
            user.Password = _dataProtection.GetPasswordHash(user.Password);
            _dataAccess.Create(user, userID);
        }

        public void UpdateUser(AppUser user)
        {
            var existingUser = _dataAccess.Get(user.ID);
            if(existingUser != null && user != null){
                existingUser.UpdateWith(user);
                _dataAccess.Update(existingUser, existingUser.ID);
            }
        }

        public void DeleteUser(string userID){
            var existingUser = _dataAccess.Get(userID);
            if(existingUser != null){                
                _dataAccess.Delete(userID);
            }            
        }
    }
}
