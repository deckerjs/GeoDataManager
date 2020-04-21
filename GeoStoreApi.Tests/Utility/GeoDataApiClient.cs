using GeoDataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoStoreApi.Tests.Utility
{
    public class GeoDataApiClient
    {
        private readonly ApiClient<GeoData> _client;

        public GeoDataApiClient(ApiClientSettings apiClientsettings)
        {
            _client = new ApiClient<GeoData>(apiClientsettings);
        }



    }
}
