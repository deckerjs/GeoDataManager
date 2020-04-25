using CoordinateDataModels;
using DataTransformUtilities.Transformers;
using GeoDataModels.Models;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.DataAccess.FileDataAccess;
using GeoStoreAPI.DataAccess.MongoDataAccess;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataStoreApiServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<IUserDataPermissionRepository, UserDataPermissionRepository>();

            services.AddScoped<IUserIdentificationService, UserIdentificationService>();
            services.AddScoped<IDataProtectionService, DataProtectionService>();

            //services.AddScoped<IGeoDataRepository, GeoDataRepository>();
            services.AddScoped<ICoordinateDataRepository, CoordinateDataRepository>();

            services.AddScoped<IGPXTransform<GeoJsonData>, GPXToGeoJsonTransform>();
            services.AddScoped<IGPXTransform<CoordinateData>, GPXToCoordinateDataTransform>();

            services.AddScoped<IQueryStringFilterBuilderService, QueryStringFilterBuilderService>();

            return services;
        }

        public static IServiceCollection AddFileDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<IFileDataAccess<AppUser>>(x => new FileDataAccess<AppUser>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<AppUser>.BASE_DIR, UserDataFileDataAccess.USER_DATA)));
            services.AddScoped<IFileDataAccess<AppRole>>(x => new FileDataAccess<AppRole>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<AppRole>.BASE_DIR, RoleDataFileDataAccess.ROLE_DATA)));
            services.AddScoped<IFileDataAccess<AppUserRoles>>(x => new FileDataAccess<AppUserRoles>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<AppUserRoles>.BASE_DIR, UserRolesFileDataAccess.USER_ROLE_DATA)));
            services.AddScoped<IFileDataAccess<UserDataPermission>>(x => new FileDataAccess<UserDataPermission>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<UserDataPermission>.BASE_DIR, UserDataPermissionFileDataAccess.USER_DATA_PERMISSION)));

            services.AddScoped<IUserDataAccess, UserDataFileDataAccess>();
            services.AddScoped<IRoleDataAccess, RoleDataFileDataAccess>();
            services.AddScoped<IUserRolesDataAccess, UserRolesFileDataAccess>();
            services.AddScoped<IUserDataPermissionDataAccess, UserDataPermissionFileDataAccess>();

            //services.AddScoped<IFileDataAccess<GeoJsonData>>(x => new FileDataAccess<GeoJsonData>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<GeoJsonData>.BASE_DIR, GeoDataFileDataAccess.DATA_GROUP)));
            //services.AddScoped<IGeoDataAccess, GeoDataFileDataAccess>();

            services.AddScoped<IFileDataAccess<CoordinateData>>(x => new FileDataAccess<CoordinateData>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<CoordinateData>.BASE_DIR, CoordinateDataFileDataAccess.DATA_GROUP)));
            services.AddScoped<ICoordinateDataAccess, CoordinateDataFileDataAccess>();

            return services;
        }

        public static IServiceCollection AddMongoDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<MongoDataContext>();
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));
            services.AddScoped<MongoSettings>(x => x.GetService<IOptions<MongoSettings>>().Value);

            services.AddScoped<IUserDataAccess, UserDataMongoDataAccess>();
            services.AddScoped<IRoleDataAccess, RoleDataMongoDataAccess>();
            services.AddScoped<IUserRolesDataAccess, UserRolesMongoDataAccess>();
            services.AddScoped<IUserDataPermissionDataAccess, UserDataPermissionMongoDataAccess>();
            services.AddScoped<ICoordinateDataAccess, CoordinateDataMongoDataAccess>();

            return services;
        }
    }
}
