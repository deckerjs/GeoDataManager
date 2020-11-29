using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Repositories
{
    public class CoordinateDataRepository
    {
        private readonly CoordinateDataSourceFactory _dataSourceFactory;

        public CoordinateDataRepository(CoordinateDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        public async Task<IEnumerable<CoordinateDataSummary>> GetTracksSummaryAsync()
        {
            //todo: inject something to do online/offline check
            var datasource = await _dataSourceFactory.GetOnlineDataSourceAsync();
            return await datasource.GetCoordinateDataSummaryAsync();
        }

        public async Task<CoordinateData> GetTrackAsync(string id)
        {
            //todo: inject something to do online/offline check
            var datasource = await _dataSourceFactory.GetOnlineDataSourceAsync();
            return await datasource.GetCoordinateDataAsync(id);
        }


    }
}
