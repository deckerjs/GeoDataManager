using XamlFreeDroidUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamlFreeDroidUI.Services
{
    public interface ISensorValuesRepository
    {
        Task<IEnumerable<SensorValueItem>> GetSensorValuesAsync();
    }
}
