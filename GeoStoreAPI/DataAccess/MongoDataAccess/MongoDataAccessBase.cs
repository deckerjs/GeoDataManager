using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public abstract class MongoDataAccessBase<T>
    {
        private readonly MongoDataContext _dataContext;
        private readonly ILogger _logger;

        public abstract string CollectionName { get; set; }
        public abstract string KeyIdName { get; set; }

        public MongoDataAccessBase(MongoDataContext dataContext, ILogger logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public void Create(T dataItem)
        {
            GetCollection().InsertOne(dataItem);
        }

        public void Delete(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(KeyIdName, id);
                var result = GetCollection().DeleteOne(filter);
                _logger.LogInformation($"Mongo DataAccess Delete Id:{id}, count:{result.DeletedCount} ");
            }
            catch (Exception ex)
            {

                _logger.LogError($"Mongo DataAccess Delete Id:{id}", ex);
            }
        }

        public T Get(string id)
        {
            var filter = Builders<T>.Filter.Eq(KeyIdName, id);
            return GetCollection().Find(filter).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Func<T, bool> funcFilter)
        {
            //todo: figure out how to give mongo a custom filter that works
            //Expression<Func<T, bool>> theFilter = x=> funcFilter(x);
            //Expression<Func<T, bool>> theFilter = Expression.Lambda<Func<T, bool>>(Expression.Equal(.Body, Expression.Constant(id)),.Parameters.First());
            //var filter = Builders<T>.Filter.Where(theFilter);

            //Temporary Hack
            var filter = Builders<T>.Filter.Where(x=>true);
            var tempResult = GetCollection().Find(filter).ToList();
            return tempResult.Where(funcFilter).ToList();            
        }

        public void Update(string id, T dataItem)
        {
            var filter = Builders<T>.Filter.Eq(KeyIdName, id);
            GetCollection().ReplaceOne(filter, dataItem);
        }

        private IMongoCollection<T> GetCollection()
        {
            return _dataContext.GetMongoCollection<T>(CollectionName);
        }
    }

}
