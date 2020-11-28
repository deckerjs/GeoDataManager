using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoStoreApi.Client
{
    public class CoordinateDataApiClient
    {
        private readonly ApiClient<CoordinateData> _client;
        private readonly ApiClient<CoordinateDataSummary> _summaryClient;
        private readonly ApiClient<Coordinate> _coordinatesClient;
        private string _baseUrl;

        private const string API_ENDPOINT = "api/CoordinateData";
        private const string API_ENDPOINT_SHARED = "shared";
        private const string API_SUMMARY_ENDPOINT = "summary";
        private const string API_SUMMARY_ENDPOINT_SHARED = "summary/shared";

        //todo: add option for getting token with provided user/pw instead of config

        public CoordinateDataApiClient()
        {
            _client = new ApiClient<CoordinateData>();
            _summaryClient = new ApiClient<CoordinateDataSummary>();
            _coordinatesClient = new ApiClient<Coordinate>();
        }

        public async Task Initialize(ApiClientSettings apiClientsettings)
        {
            _baseUrl = $"{apiClientsettings.BaseUrl}/{API_ENDPOINT}";
            await _client.Initialize(apiClientsettings);
            await _coordinatesClient.Initialize(apiClientsettings);
        }

        public async Task<IEnumerable<CoordinateData>> GetAll()
        {
            return await _client.GetCollection(_baseUrl);
        }

        public async Task<CoordinateData> GetById(string id)
        {
            string url = $"{_baseUrl}/{id}";
            return await _client.Get(url);
        }

        public async Task<IEnumerable<CoordinateData>> GetShared()
        {
            string url = $"{_baseUrl}/{API_ENDPOINT_SHARED}";
            return await _client.GetCollection(url);
        }

        public async Task<IEnumerable<CoordinateDataSummary>> GetSummary()
        {
            string url = $"{_baseUrl}/{API_SUMMARY_ENDPOINT}";
            return await _summaryClient.GetCollection(url);
        }

        public async Task<IEnumerable<CoordinateDataSummary>> GetSummaryShared()
        {
            string url = $"{_baseUrl}/{API_SUMMARY_ENDPOINT_SHARED}";
            return await _summaryClient.GetCollection(url);
        }

        public async Task<IEnumerable<Coordinate>> GetCoordinates(string id, string pointCollectionId)
        {
            string url = $"{_baseUrl}/{id}/data/{pointCollectionId}/coordinates";
            return await _coordinatesClient.GetCollection(url);
        }
               
        public async Task<string> Post(CoordinateData geoData)
        {
            return await _client.Post(_baseUrl, geoData);
        }

        public async Task<string> AppendCoordinatesTpPointCollection(string id, string pointCollectionId, List<Coordinate> coordinates)
        {
            string url = $"{_baseUrl}/{id}/data/{pointCollectionId}/coordinates";
            return await _coordinatesClient.PostCollection(url, coordinates);
        }
        public async Task<string> PostCoordinatesAndCreateNewCoordinateDataitem(List<Coordinate> coordinates)
        {
            string url = $"{_baseUrl}/data/coordinates";
            return await _coordinatesClient.PostCollection(url, coordinates);
        }

        public async Task Put(string id, CoordinateData data)
        {
            string url = $"{_baseUrl}/{id}";
            await _client.Put(url, data);
        }
        
        public async Task Delete(string id)
        {
            string url = $"{_baseUrl}/{id}";
            await _client.Delete(url);
        }

    }
}
