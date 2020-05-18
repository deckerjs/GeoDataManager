using GeoDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.Services
{
    public interface IQueryStringFilterBuilderService
    {
        List<Expression<Func<T, bool>>> GetFilter<T>();
    }
}
