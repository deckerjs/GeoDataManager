using System.IO;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder 
        AddIdentityServices(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IFileDataAccess<AppUser>>(x => new FileDataAccess<AppUser>(Path.Combine(Directory.GetCurrentDirectory(), UserDataAccess.USER_DATA)));
            builder.Services.AddScoped<IFileDataAccess<AppRole>>(x => new FileDataAccess<AppRole>(Path.Combine(Directory.GetCurrentDirectory(), RoleDataAccess.ROLE_DATA)));
            builder.Services.AddScoped<IFileDataAccess<AppUserRoles>>(x => new FileDataAccess<AppUserRoles>(Path.Combine(Directory.GetCurrentDirectory(), UserRolesDataAccess.USER_ROLE_DATA)));

            builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();
            builder.Services.AddScoped<IRoleDataAccess, RoleDataAccess>();
            builder.Services.AddScoped<IUserRolesDataAccess, UserRolesDataAccess>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRolesRepository, UserRolesRepository>();

            builder.AddProfileService<CustomProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();
            builder.Services.AddScoped<IUserIdentificationService, UserIdentificationService>();

            return builder;
        }
    }
}