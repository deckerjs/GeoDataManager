using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Func<T, bool> GetFilter<T>()
        {
            var queryPrm = _context.HttpContext.Request.Query;
            var props = typeof(T).GetProperties().ToDictionary(k => k.Name, v => v);
            List<Func<T, bool>> filters = new List<Func<T, bool>>();

            foreach (var filterProp in queryPrm)
            {
                if (props.ContainsKey(filterProp.Key) && !string.IsNullOrEmpty(filterProp.Value))
                {
                    Func<T, bool> filter = GetFunc<T>(props, filterProp);
                    filters.Add(filter);
                }
            }

            return (x => !filters.Where(f => f(x) == false).Any());

        }

        private static Func<T, bool> GetFunc<T>(Dictionary<string, PropertyInfo> props, KeyValuePair<string, StringValues> filterProp)
        {
            //todo: check for collection types and add linq filter

            return (x =>
                {
                    string itemVal = props[filterProp.Key].GetValue(x).ToString();

                    string compareVal = filterProp.Value.ToString().Trim();
                    compareVal = compareVal.Length <= 255 ? compareVal : compareVal.Substring(0, 255);

                    return itemVal == compareVal;
                });
        }
    }
}
