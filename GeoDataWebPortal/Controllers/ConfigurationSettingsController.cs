using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoDataWebPortal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoDataWebPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationSettingsController : ControllerBase
    {
        private readonly ClientConfigurationSettings _settings;

        public ConfigurationSettingsController(ClientConfigurationSettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public ClientConfigurationSettings Get()
        {
            return _settings;
        }
        

    }
}
