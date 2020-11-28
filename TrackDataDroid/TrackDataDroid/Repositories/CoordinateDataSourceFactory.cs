using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Repositories
{
    public class CoordinateDataSourceFactory
    {
        public CoordinateDataSourceFactory()
        {

        }

        public async Task<ICoordinateDataSource> GetOnlineDataSourceAsync()
        {
            ICoordinateDataSource source = App.Host.Services.GetRequiredService<CoordinateDataOnlineSource>();
            await source.InitializeAsync();
            return source;
        }
        
        public async Task<ICoordinateDataSource> GetOfflineDataSourceAsync()
        {
            ICoordinateDataSource source = App.Host.Services.GetRequiredService<CoordinateDataOfflineSource>();
            await source.InitializeAsync();
            return source;
        }


    }
}
