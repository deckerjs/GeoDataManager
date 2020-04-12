using GeoDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Services
{
    public interface IQueryStringFilterBuilderService
    {
        Func<T, bool> GetFilter<T>();
    }
}
