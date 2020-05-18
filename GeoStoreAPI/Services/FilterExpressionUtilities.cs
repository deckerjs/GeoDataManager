using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.Services
{
    public static class FilterExpressionUtilities
    {
        public static Expression<Func<T, bool>> GetEqExpressionForProperty<T>(string propertyName, string compareValue)
        {
            string compareVal = compareValue.Trim();
            compareVal = compareVal.Length <= 255 ? compareVal : compareVal.Substring(0, 255);

            var param = Expression.Parameter(typeof(T), "p");
            var body = Expression.Equal(
            Expression.PropertyOrField(param, propertyName),
            Expression.Constant(compareVal)
            );

            var lambda = Expression.Lambda<Func<T, bool>>(body, param);

            return lambda;
        }


        public static Expression<Func<T, bool>> GetEqExpressionForProperty<T>(string propertyName, bool compareValue)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var body = Expression.Equal(
            Expression.PropertyOrField(param, propertyName),
            Expression.Constant(compareValue)
            );

            var lambda = Expression.Lambda<Func<T, bool>>(body, param);

            return lambda;
        }

    }
}
