using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataDroid.Repositories
{
    public interface ICoordinateDataSource
    {
        Task InitializeAsync();
        Task<IEnumerable<CoordinateDataSummary>> GetCoordinateDataSummaryAsync();
        Task<CoordinateData> GetCoordinateDataAsync(string id);
    }
}
