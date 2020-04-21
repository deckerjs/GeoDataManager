using GeoStoreApi.Tests.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using GeoDataModels.Models;

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
        public void GeoDataTest1()
        {
            var apiClient = new GeoDataApiClient(_clientSettings);



            Assert.Pass();
        }

        private ApiClientSettings GetApiClientSettings(IConfiguration config)
        {
            return config.GetSection("ApiClientSettings").Get<ApiClientSettings>();
        }


    }
}