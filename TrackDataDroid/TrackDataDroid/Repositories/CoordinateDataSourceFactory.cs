using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Repositories
{
    public class CoordinateDataSourceFactory
    {
        private ICoordinateDataSource _onlineSource;
        private ICoordinateDataSource _offlineSource;

        public CoordinateDataSourceFactory(){}

        public async Task<ICoordinateDataSource> GetOnlineDataSourceAsync()
        {
            if (_onlineSource == null)
            {
                _onlineSource = App.Host.Services.GetRequiredService<CoordinateDataOnlineSource>();
                await _onlineSource.InitializeAsync();
            }

            return _onlineSource;
        }
        
        public async Task<ICoordinateDataSource> GetOfflineDataSourceAsync()
        {
            _offlineSource = App.Host.Services.GetRequiredService<CoordinateDataOfflineSource>();
            await _offlineSource.InitializeAsync();
            return _offlineSource;
        }


    }
}
