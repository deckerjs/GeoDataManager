using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.MongoDataAccess
{
    public class MongoDataContext
    {
        private readonly IMongoDatabase _db;

        public MongoDataContext(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _db = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<T> GetMongoCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

    }


}
