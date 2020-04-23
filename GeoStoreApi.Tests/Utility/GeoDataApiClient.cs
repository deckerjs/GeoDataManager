using GeoDataModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoStoreApi.Tests.Utility
{
    public class GeoDataApiClient
    {
        private readonly ApiClient<GeoData> _client;
        private readonly ApiClient<Coordinate> _coordinatesClient;
        private string _baseUrl;

        public GeoDataApiClient(ApiClientSettings apiClientsettings)
        {
            _client = new ApiClient<GeoData>(apiClientsettings);
            _coordinatesClient = new ApiClient<Coordinate>(apiClientsettings);
            _baseUrl = "apiClientsettings.BaseUrl/api/GeoData";
        }
                
        public async Task<IEnumerable<GeoData>> GetAll()
        {
            return await _client.GetCollection(_baseUrl);
        }

        public async Task<GeoData> GetById(string id)
        {
            string url = $"{_baseUrl}/{id}";
            return await _client.Get(url);
        }

        public async Task<IEnumerable<GeoData>> GetShared()
        {
            string url = $"{_baseUrl}/shared";
            return await _client.GetCollection(url);
        }

        public async Task<IEnumerable<Coordinate>> GetCoordinates(string id)
        {
            string url = $"{_baseUrl}/{id}/data/features/geometry/coordinates";
            return await _coordinatesClient.GetCollection(url);
        }
               
        public async Task<string> Post(GeoData geoData)
        {
            return await _client.Post(_baseUrl, geoData);
        }

        public async Task<string> PostCoordinates(string id, List<Coordinate> coordinates)
        {
            string url = $"{_baseUrl}/{id}/data/features/geometry/coordinates";
            return await _coordinatesClient.PostCollection(url, coordinates);
        }
        public async Task<string> PostCoordinatesToNewItem(List<Coordinate> coordinates)
        {
            string url = $"{_baseUrl}/data/features/geometry/coordinates";
            return await _coordinatesClient.PostCollection(url, coordinates);
        }

        public async Task Put(string id, GeoData data)
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
