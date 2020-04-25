using CoordinateDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess
{
    public interface ICoordinateDataAccess
    {
        void Create(CoordinateData geoData);
        CoordinateData Get(string id);
        void Delete(string id);
        IEnumerable<CoordinateData> GetAll(Func<CoordinateData, bool> filter);
        void Update(string id, CoordinateData geoData);
    }
}
