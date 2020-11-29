using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Repositories
{
    public class CoordinateDataOfflineSource : ICoordinateDataSource
    {
        public CoordinateDataOfflineSource()
        {

        }

        public async Task InitializeAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<CoordinateDataSummary>> GetCoordinateDataSummaryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CoordinateData> GetCoordinateDataAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
