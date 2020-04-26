using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoStoreApi.Tests.Utility
{
    public class CoordinateDataApiClient
    {
        private readonly ApiClient<CoordinateData> _client;
        private readonly ApiClient<Coordinate> _coordinatesClient;
        private string _baseUrl;

        public CoordinateDataApiClient(ApiClientSettings apiClientsettings)
        {
            _client = new ApiClient<CoordinateData>(apiClientsettings);
            _coordinatesClient = new ApiClient<Coordinate>(apiClientsettings);
            _baseUrl = $"{apiClientsettings.BaseUrl}/api/CoordinateData";
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
            string url = $"{_baseUrl}/shared";
            return await _client.GetCollection(url);
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
            string url = $"{_baseUrl}/data/features/geometry/coordinates";
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
