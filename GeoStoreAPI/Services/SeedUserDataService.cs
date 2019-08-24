using System;
using System.Collections.Generic;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using Microsoft.AspNetCore.Builder;

namespace GeoStoreAPI.Services
{
    public static class SeedUserDataService 
    {
        private static AppOptions _options;
        private static IUserRolesRepository _userRolesRepository;
        private static IUserRepository _userRepository;
        private static IRoleRepository _roleRepository;

        public static IApplicationBuilder SeedUserData(this IApplicationBuilder app, IServiceProvider serviceProvider){

            _options = serviceProvider.GetService(typeof(AppOptions)) as AppOptions;            
            _userRolesRepository = serviceProvider.GetService(typeof(IUserRolesRepository)) as IUserRolesRepository;
            _userRepository = serviceProvider.GetService(typeof(IUserRepository)) as IUserRepository;
            _roleRepository = serviceProvider.GetService(typeof(IRoleRepository)) as IRoleRepository;

            return app;
        }
        
        public static void Start()
        {
            if (_options.GenerateDefaultUsers == true)
            {
                CreateDefaultRolesIfAbsent();
                CreateDefaultUsersIfAbsent();
                CreateDefaultUserRolesIfAbsent();
            }
        }

        private static void CreateDefaultRolesIfAbsent()
        {
            List<AppRole> _roles = new List<AppRole>
            {
                new AppRole{
                    RoleID = "user",
                    Description = "a user"
                },
                new AppRole{
                    RoleID = "admin",
                    Description = "an admin"
                }
            };

            _roles.ForEach(role =>
            {
                if (_roleRepository.GetRole(role.RoleID) == null)
                {
                    _roleRepository.CreateRole(role);
                }
            });
        }

        public static void CreateDefaultUsersIfAbsent()
        {
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
                },
                new AppUser{
                    ID = "100003",
                    UserName = "admin1",
                    Password = "password1",
                    Email = "admin1@email.com"
                }
            };

            _users.ForEach(user =>
            {
                if (_userRepository.FindBySubjectId(user.ID) == null)
                {
                    _userRepository.CreateUser(user.ID, user);
                }
            });
        }

        private static void CreateDefaultUserRolesIfAbsent()
        {

            List<AppUserRoles> _userRoles = new List<AppUserRoles>
            {
                new AppUserRoles{
                    UserID = "100000",
                    RoleIDs = new List<string>() {"user"}
                },
                new AppUserRoles{
                    UserID = "100001",
                    RoleIDs = new List<string>() {"user"}
                },
                new AppUserRoles{
                    UserID = "100003",
                    RoleIDs = new List<string>() {"user", "admin"}
                }
            };

            _userRoles.ForEach(userRole =>
            {
                if (_userRolesRepository.GetUserRoles(userRole.UserID) == null)
                {
                    _userRolesRepository.CreateUserRoles(userRole);
                }
            });
        }

    }
}