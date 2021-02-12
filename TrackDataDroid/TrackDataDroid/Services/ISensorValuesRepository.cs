using TrackDataDroid.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Services
{
    public interface ISensorValuesRepository
    {
        Task<IEnumerable<SensorValueItem>> GetSensorValuesAsync();
    }
}
