using GeoStoreApi.Tests.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using GeoDataModels.Models;
using System.Threading.Tasks;

namespace GeoStoreApi.Tests
{
    public class Tests
    {        
        private ApiClientSettings _clientSettings;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
               .AddUserSecrets<ApiClientSettings>()
               .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            _clientSettings = GetApiClientSettings(configuration);
        }

        [Test]
        public async Task GeoDataTest1()
        {
            var api = new GeoDataApiClient(_clientSettings);

            var newData = new GeoData()
            {
                Description = "test item 1",
                Tags = { "test tag 1", "test tag 2" },
                Data = GetTestFeatureCollection()
            };

            var newId = await api.Post(newData);

            Assert.IsFalse(string.IsNullOrEmpty(newId));

            var existingData = await api.GetById(newId);

            Assert.IsTrue(existingData != null && existingData.Data.Features.Any());

            MultiPoint mpGeom = (MultiPoint)existingData.Data.Features.First().Geometry;

            Assert.IsTrue(mpGeom != null && mpGeom.Coordinates != null && mpGeom.Coordinates.Any());

            //todo: This is why I need to get rid of the GeoJson dependency for my models.
            var newGodDamnCoordCollectionBecausePropertyIsGetOnly = (MultiPoint)mpGeom.Coordinates.Append(new Position(10, 10));
            var newGodDamnFeatureCollection = new FeatureCollection(new List<Feature> { new Feature(newGodDamnCoordCollectionBecausePropertyIsGetOnly) });

            existingData.Data = newGodDamnFeatureCollection;

            await api.Put(newId, existingData);

            await api.Delete(newId);

            var dataStillExists = await api.GetById(newId);

            Assert.IsNull(dataStillExists);

            Assert.Pass();
        }

        private ApiClientSettings GetApiClientSettings(IConfiguration config)
        {
            return config.GetSection("ApiClientSettings").Get<ApiClientSettings>();
        }

        private FeatureCollection GetTestFeatureCollection()
        {
            var props = new Dictionary<string, object>();

            props["Name"] = "prop 1 name";

            var coords = new List<Position>(){
                 new Position(-88.095398000000003, 42.918624000000001, 252.93600000000001),
                 new Position(-88.095395999999994, 42.918604000000002, 245.953),
                 new Position(-88.095110000000005, 42.918570000000003, 241.78899999999999)
             };

            var geom1 = new MultiPoint(coords);
            var feature1 = new Feature(geom1, props);
            
            var features = new List<Feature>() { feature1 };
            
            var data = new FeatureCollection(features);
            return data;
        }



    }
}