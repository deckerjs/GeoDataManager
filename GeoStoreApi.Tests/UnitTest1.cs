using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using CoordinateDataModels;
using GeoStoreApi.Client;
using System.Reflection;

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
               .AddUserSecrets(typeof(Tests).Assembly)
               .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            _clientSettings = GetApiClientSettings(configuration);
        }

        [Test]
        public async Task GeoDataTest1()
        {
            var api = new CoordinateDataApiClient();
            await api.Initialize(_clientSettings);

            var newData = new CoordinateData()
            {
                Description = "test item 1",
                Tags = new List<string> { "test tag 1", "test tag 2" },
                Data = GetTestPointCollection()
            };

            var newId = await api.Post(newData);

            Assert.IsFalse(string.IsNullOrEmpty(newId));

            var existingData = await api.GetById(newId);

            Assert.IsTrue(existingData != null && existingData.Data.Any());

            var existingPointCollection = existingData.Data.First();

            int originalCoordCount = existingPointCollection.Coordinates.Count();

            existingPointCollection.Coordinates.AddRange(GetTestPointCollection().First().Coordinates);

            int newCoordCount = existingPointCollection.Coordinates.Count();

            await api.Put(newId, existingData);

            var updatedData = await api.GetById(newId);

            Assert.AreEqual(newCoordCount, updatedData.Data.Where(x => x.ID == existingPointCollection.ID).First().Coordinates.Count());

            await api.Delete(newId);

            var dataStillExists = await api.GetById(newId);

            Assert.IsNull(dataStillExists);

            Assert.Pass();
        }

        [Test]
        public async Task GeoDataAppendCoordinatesTest()
        {
            var api = new CoordinateDataApiClient();
            await api.Initialize(_clientSettings);

            var newData = new CoordinateData()
            {
                Description = "test item 2",
                Tags = new List<string> { "test tag 2", "test tag 3" },
                Data = GetTestPointCollection()
            };

            var newId = await api.Post(newData);
            Assert.IsFalse(string.IsNullOrEmpty(newId));
            var existingData = await api.GetById(newId);
            Assert.IsTrue(existingData != null && existingData.Data.Any());
            
            var existingPointCollection = existingData.Data.First();

            int originalCoordCount = existingPointCollection.Coordinates.Count();

            var coordsToAdd = GetTestPointCollection().First().Coordinates;
            int addedCoordCount = coordsToAdd.Count();

            await api.AppendCoordinatesTpPointCollection(newId, existingPointCollection.ID, coordsToAdd);

            var updatedData = await api.GetById(newId);

            Assert.AreEqual((originalCoordCount+addedCoordCount), updatedData.Data.Where(x => x.ID == existingPointCollection.ID).First().Coordinates.Count());

            await api.Delete(newId);

            var dataStillExists = await api.GetById(newId);

            Assert.IsNull(dataStillExists);

            Assert.Pass();
        }

        private ApiClientSettings GetApiClientSettings(IConfiguration config)
        {
            return config.GetSection("ApiClientSettings").Get<ApiClientSettings>();
        }

        private List<PointCollection> GetTestPointCollection()
        {
            List<PointCollection> pointCollection = new List<PointCollection>();

            var props = new Dictionary<string, string>();
            var telemetryData = new Dictionary<string, double>();

            props["Name"] = "prop 1 name";

            var coords = new List<Coordinate>(){
                 new Coordinate(-88.095398000000003, 42.918624000000001, 252.93600000000001, DateTime.Now),
                 new Coordinate(-88.095395999999994, 42.918604000000002, 245.953, DateTime.Now),
                 new Coordinate(-88.095110000000005, 42.918570000000003, 241.78899999999999, DateTime.Now)
             };

            coords.AddRange(new[]{
                 new Coordinate(-88.095398000000003, 42.918624000000001, 4940.6167, DateTime.Now,
                 new Dictionary<string, double>()
                 {
                     ["Quality"]=1,
                     ["Heading"]=144.6,
                     ["FeetPerSecond"]=33.92,
                     ["SatellitesInView"]=13,
                     ["SignalToNoiseRatio"]=50,
                     ["Hdop"]=.7
                 }),
                 new Coordinate(-88.095395999999994, 42.918604000000002, 245.953, DateTime.Now,
                 new Dictionary<string, double>()
                 {
                     ["Quality"]=1,
                     ["Heading"]=144.6,
                     ["FeetPerSecond"]=33.92,
                     ["SatellitesInView"]=13,
                     ["SignalToNoiseRatio"]=50,
                     ["Hdop"]=.7
                 }),
                 new Coordinate(-88.095110000000005, 42.918570000000003, 241.78899999999999, DateTime.Now,
                 new Dictionary<string, double>()
                 {
                     ["Quality"]=1,
                     ["Heading"]=144.6,
                     ["FeetPerSecond"]=33.92,
                     ["SatellitesInView"]=13,
                     ["SignalToNoiseRatio"]=50,
                     ["Hdop"]=.7
                 })
             });

            pointCollection.Add(new PointCollection()
            {
                Coordinates = coords,
                ID = "1234",
                Metadata = props
            });

            return pointCollection;
        }
    }
}