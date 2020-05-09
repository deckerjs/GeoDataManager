using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoStoreApi.Client
{
    internal class ApiClient<T>
    {
        private HttpClient _client;
        private ApiClientSettings _apiClientsettings;
        private TokenResponse _lastTokenResponse;
        DateTime _tokenExpires;
        private int _minutesAheadOfExpireToRefresh = 5;

        public ApiClient(){}

        public async Task Initialize(ApiClientSettings apiClientsettings)
        {
            _apiClientsettings = apiClientsettings;
            await SetupHttpClient(apiClientsettings);
        }
                
        public async Task<string> Post(string url, T body)
        {
            await CheckTokenExpiration();

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

        public async Task<string> PostCollection(string url, IEnumerable<T> body)
        {
            await CheckTokenExpiration();

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
            await CheckTokenExpiration();

            var response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                T results = JsonSerializer.Deserialize<T>(content);
                return results;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            else
            {
                ThrowBadResponse(response);
            }

            return default;
        }

        public async Task<IEnumerable<T>> GetCollection(string url)
        {
            await CheckTokenExpiration();

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
            await CheckTokenExpiration();

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
            await CheckTokenExpiration();

            var response = await _client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ThrowBadResponse(response);
            }
        }

        private async Task SetupHttpClient(ApiClientSettings settings)
        {
            _client = new HttpClient();
            var response = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                UserName = settings.UserID,
                Password = settings.Password,
                Address = settings.TokenEndpoint,
                ClientId = settings.ClientID,
                ClientSecret = settings.ClientSecret,
                Scope = settings.ClientScope
            });

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }
            else
            {
                SetToken(response);
            }
        }
        
        private async Task CheckTokenExpiration()
        {
            if (_tokenExpires < DateTime.Now.AddMinutes(_minutesAheadOfExpireToRefresh))
            {
                await RefreshToken();
            }
        }

        private async Task RefreshToken()
        {
            var response = await _client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                RefreshToken = _lastTokenResponse.RefreshToken,
                Address = _apiClientsettings.TokenEndpoint,
                ClientId = _apiClientsettings.ClientID,
                ClientSecret = _apiClientsettings.ClientSecret,
                Scope = _apiClientsettings.ClientScope
            });

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }
            else
            {
                SetToken(response);
            }
        }

        private void SetToken(TokenResponse response)
        {
            _client.SetBearerToken(response.AccessToken);
            _tokenExpires = DateTime.Now.AddSeconds(response.ExpiresIn);
            _lastTokenResponse = response;
        }

        private void ThrowBadResponse(HttpResponseMessage response)
        {
            Exception innerExpeption = new Exception(response.Content.ReadAsStringAsync().Result);
            throw new Exception(response.ReasonPhrase, innerExpeption);
        }
    }

}
