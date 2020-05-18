using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.FileDataAccess
{
    public interface IFileDataAccess<T>
    {
        IEnumerable<T> GetAllItems(string storageGroup, IEnumerable<Expression<Func<T, bool>>> filter);
        void DeleteAllItems(string storageGroup);
        T GetItem(string storageGroup, string name);
        void CreateItem(string storageGroup, string name, T updateItem);
        void SaveItem(string storageGroup, string name, T updateItem);
        void DeleteItem(string storageGroup, string name);
    }
}
