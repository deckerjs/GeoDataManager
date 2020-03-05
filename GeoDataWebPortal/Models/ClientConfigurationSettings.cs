using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoDataWebPortal.Models
{
    public class ClientConfigurationSettings
    {
        public string AuthUrl { get; set; }
        public string AuthClientSecret { get; set; }
        public string GeoDataApiUrl { get; set; }
        public string MapboxToken { get; set; }
    }
}
