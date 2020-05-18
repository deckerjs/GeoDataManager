using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using GeoStoreAPI.Models;
using GeoStoreAPI.DataAccess;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using GeoStoreAPI.Services;
using System.Linq.Expressions;

namespace GeoStoreAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDataAccess _dataAccess;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly AppOptions _options;
        private readonly IDataProtectionService _dataProtection;

        public UserRepository(IUserDataAccess dataAccess,
            IUserRolesRepository userRolesRepository,
            AppOptions options, 
            IDataProtectionService dataProtection)
        {
            _dataAccess = dataAccess;
            _userRolesRepository = userRolesRepository;
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

        public IEnumerable<AppUser> GetAllUsers(IEnumerable<Expression<Func<AppUser, bool>>> filter)
        {
            return _dataAccess.GetAll(filter);
        }

        public AppUser GetUser(string subjectId)
        {
            var filter = FilterExpressionUtilities.GetEqExpressionForProperty<AppUser>("ID", subjectId);
            var users = _dataAccess.GetAll(new[] { filter });
            if(users!=null)return users.FirstOrDefault();
            return null;
        }

        public AppUser FindByUsername(string username)
        {
            var filters = new List<Expression<Func<AppUser, bool>>>();
            filters.Add(FilterExpressionUtilities.GetEqExpressionForProperty<AppUser>("UserName", username));
            var users = _dataAccess.GetAll(filters);
            return users.FirstOrDefault();
        }

        public void CreateUser(string userID, AppUser user)
        {
            user.Password = _dataProtection.GetPasswordHash(user.Password);
            user.ID = userID;
            _dataAccess.Create(user);
        }

        public void CreateUser(AppUser user)
        {
            string userID = Guid.NewGuid().ToString();
            user.Password = _dataProtection.GetPasswordHash(user.Password);
            user.ID = userID;
            _dataAccess.Create(user);
            _userRolesRepository.CreateUserRoles(new AppUserRoles() { UserID = user.ID, RoleIDs = { "user" } });
        }

        public void UpdateUser(AppUser user)
        {
            var existingUser = _dataAccess.Get(user.ID);
            if(existingUser != null && user != null){
                existingUser.UpdateWith(user);
                existingUser.Password = _dataProtection.GetPasswordHash(user.Password);                
                _dataAccess.Update(existingUser.ID, existingUser);
            }
        }

        public void DeleteUser(string userID){
            var existingUser = _dataAccess.Get(userID);
            if(existingUser != null){                
                _dataAccess.Delete(userID);
                _userRolesRepository.RemoveUserRoles(userID);
            }            
        }
    }
}
