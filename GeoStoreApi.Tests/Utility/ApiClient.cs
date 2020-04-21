using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoStoreApi.Tests.Utility
{
    public class ApiClient<T>
    {
        private HttpClient _client;

        public ApiClient(ApiClientSettings apiClientsettings)
        {
            _client = GetHttpClient(apiClientsettings);
        }

        private HttpClient GetHttpClient(ApiClientSettings settings)
        {
            var client = new HttpClient();
            var response = client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                UserName = settings.UserID,
                Password = settings.Password,
                Address = settings.TokenEndpoint,
                ClientId = settings.ClientID,
                ClientSecret = settings.ClientSecret,
                Scope = settings.ClientScope
            }).Result;

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }
            else
            {
                client.SetBearerToken(response.AccessToken);
            }

            return client;
        }

        public async Task<string> Post(string url, T body)
        {
            string json = JsonSerializer.Serialize(body);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                ThrowBadResponse(response);
            }

            return default;
        }

        public async Task<T> Get(string url)
        {
            var response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                T results = JsonSerializer.Deserialize<T>(content);
                return results;
            }
            else
            {
                ThrowBadResponse(response);
            }

            return default;
        }

        public async Task<IEnumerable<T>> GetCollection(string url)
        {
            var response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                IEnumerable<T> results = JsonSerializer.Deserialize<IEnumerable<T>>(content);
                return results;
            }
            else
            {
                ThrowBadResponse(response);
            }

            return default;
        }

        public async Task Put(string url, T body)
        {
            string json = JsonSerializer.Serialize(body);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                ThrowBadResponse(response);
            }
        }


        public async Task Delete(string url)
        {
            var response = await _client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ThrowBadResponse(response);
            }
        }

        private void ThrowBadResponse(HttpResponseMessage response)
        {
            Exception innerExpeption = new Exception(response.Content.ReadAsStringAsync().Result);
            throw new Exception(response.ReasonPhrase, innerExpeption);
        }
    }

}
