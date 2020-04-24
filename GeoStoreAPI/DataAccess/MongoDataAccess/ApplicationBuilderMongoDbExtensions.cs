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
            // public MyClassSerializer : IBsonSerializer {
            // implement Deserialize
            // implement GetDefaultSerializationOptions
            // implement Serialize
            //}

            //// register your custom serializer
            //BsonSerializer.RegisterSerializer(
            //typeof(MyClass),
            //new MyClassSerializer()
            //);


            if (!BsonClassMap.IsClassMapRegistered(typeof(GeoData)))
            {

                //custom serializer
                BsonClassMap.RegisterClassMap<GeoData>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(c => c.Data).SetSerializer(new CustomGeoJsonSerializer());
                    cm.SetIgnoreExtraElements(true);
                });

                //BsonClassMap.RegisterClassMap<GeoData>(cm =>
                //{
                //    cm.AutoMap();
                //    //for readonly properties, since by default they will be ignored
                //    //BsonClassMap.RegisterClassMap<GeoData>(cm => {
                //    //    cm.MapProperty(c => c.SomeProperty);
                //    //});
                //    cm.SetIgnoreExtraElements(true);
                //});
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
