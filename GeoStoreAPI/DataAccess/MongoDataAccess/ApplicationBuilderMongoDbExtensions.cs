using CoordinateDataModels;
using GeoDataModels.Models;
using GeoStoreAPI.Models;
using Microsoft.AspNetCore.Builder;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Extensions
{
    public static class ApplicationBuilderMongoDbExtensions
    {
        public static IApplicationBuilder InitializeMongo(this IApplicationBuilder app)
        {
            AddBsonMapConfigurations();
            return app;
        }

        private static void AddBsonMapConfigurations()
        {   
            if (!BsonClassMap.IsClassMapRegistered(typeof(CoordinateData)))
            {
                BsonClassMap.RegisterClassMap<CoordinateData>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(CoordinateDataSummary)))
            {
                BsonClassMap.RegisterClassMap<CoordinateDataSummary>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(AppRole)))
            {
                BsonClassMap.RegisterClassMap<AppRole>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(AppUser)))
            {
                BsonClassMap.RegisterClassMap<AppUser>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(UserDataPermission)))
            {
                BsonClassMap.RegisterClassMap<UserDataPermission>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(AppUserRoles)))
            {
                BsonClassMap.RegisterClassMap<AppUserRoles>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }

    }
}
