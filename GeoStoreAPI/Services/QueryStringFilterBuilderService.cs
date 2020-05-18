using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace GeoStoreAPI.Services
{
    public class QueryStringFilterBuilderService : IQueryStringFilterBuilderService
    {
        private readonly IHttpContextAccessor _context;

        public QueryStringFilterBuilderService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public List<Expression<Func<T, bool>>> GetFilter<T>()
        {
            var queryPrm = _context.HttpContext.Request.Query;
            var props = typeof(T).GetProperties().ToDictionary(k => k.Name, v => v);
            List<Expression<Func<T, bool>>> filters = new List<Expression<Func<T, bool>>>();

            foreach (var filterProp in queryPrm)
            {
                if (props.ContainsKey(filterProp.Key) && !string.IsNullOrEmpty(filterProp.Value))
                {
                    Expression<Func<T, bool>> filter = FilterExpressionUtilities.GetEqExpressionForProperty<T>(filterProp.Key, filterProp.Value.ToString());
                    filters.Add(filter);
                }
            }

            return filters;
        }

    }
}
