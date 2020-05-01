using System;
using System.Collections.Generic;
using System.Text;

namespace GeoStoreApi.Client
{
    public class ApiClientSettings
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string TokenEndpoint { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string ClientScope { get; set; }
        public string BaseUrl { get; set; }
    }
}
