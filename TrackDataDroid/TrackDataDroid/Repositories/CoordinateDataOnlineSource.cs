using CoordinateDataModels;
using GeoStoreApi.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Repositories
{
    public class CoordinateDataOnlineSource : ICoordinateDataSource
    {
        private readonly CoordinateDataApiClient _apiClient;
        private readonly ApiClientSettings _apiClientsettings;

        public CoordinateDataOnlineSource(CoordinateDataApiClient apiClient, ApiClientSettings apiClientsettings)
        {
            _apiClient = apiClient;
            _apiClientsettings = apiClientsettings;
        }

        public async Task InitializeAsync()
        {
            await _apiClient.Initialize(_apiClientsettings);
        }

        public async Task<IEnumerable<CoordinateDataSummary>> GetCoordinateDataSummaryAsync()
        {
            return await _apiClient.GetSummary();
        }

    }
}
