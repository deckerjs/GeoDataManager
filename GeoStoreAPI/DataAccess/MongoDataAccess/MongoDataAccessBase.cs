using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcFilter">
        /// Be careful not to pass in expressions that include lambdas that have code that mongo can't execute, like reflection
        /// </param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(IEnumerable<Expression<Func<T, bool>>> funcFilter)
        {
            FilterDefinition<T> fd = null;

            if (funcFilter!=null && funcFilter.Any())
            {
                var filter = Builders<T>.Filter;

                foreach (var item in funcFilter)
                {
                    if (fd == null)
                    {
                        fd = Builders<T>.Filter.Where(item);
                    }
                    else
                    {
                        fd = fd & Builders<T>.Filter.Where(item);
                    }
                }
            }
            else
            {
                fd = Builders<T>.Filter.Where(x=>true);
            }

            return GetCollection().Find(fd).ToList();
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
