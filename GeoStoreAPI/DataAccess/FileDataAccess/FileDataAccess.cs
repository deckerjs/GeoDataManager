using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeoStoreAPI.DataAccess.FileDataAccess
{
    public class FileDataAccess<T> : IFileDataAccess<T>
    {
        public const string BASE_DIR = "data";
        private readonly string _mainPath;

        public FileDataAccess(string mainPath)
        {
            _mainPath = mainPath;
            Initialize();
        }

        private void Initialize()
        {
            if (!Directory.Exists(_mainPath))
            {
                Directory.CreateDirectory(_mainPath);
            }
        }

        public void CreateItem(string storageGroup, string name, T item)
        {
            WriteFile(storageGroup, name, item);
        }

        public void DeleteAllItems(string storageGroup)
        {
            string dirPathString = System.IO.Path.Combine(_mainPath, storageGroup);
            if (System.IO.Directory.Exists(dirPathString))
            {
                var files = Directory.GetFiles(dirPathString);
                foreach (var item in files)
                {
                    File.Delete(item);
                }
            }
        }

        public void DeleteItem(string storageGroup, string name)
        {
            string dirPathString = System.IO.Path.Combine(_mainPath, storageGroup);
            string fileName = $"{name}.json";
            string filePathString = Path.Combine(dirPathString, fileName);

            if (System.IO.Directory.Exists(dirPathString) && File.Exists(filePathString))
            {
                File.Delete(filePathString);
            }
        }

        public IEnumerable<T> GetAllItems(string storageGroup, IEnumerable<Expression<Func<T, bool>>> filter)
        {
            var files = GetAllFiles(storageGroup).Select(x => ReadItemFromFile(storageGroup, x)).Where(x => x != null).ToList();
            if (files != null && files.Any())
            {
                return files.Where(x => filter.All(y => y.Compile()(x) == true));
            }
            else return null;
        }

        public T GetItem(string storageGroup, string name)
        {
            return ReadItemFromFile(storageGroup, name);
        }

        public void SaveItem(string storageGroup, string name, T item)
        {
            WriteFile(storageGroup, name, item);
        }

        private void WriteFile(string dir, string name, T item)
        {
            string dirPathString = Path.Combine(_mainPath, dir);
            string fileName = $"{name}.json";
            string filePathString = Path.Combine(dirPathString, fileName);

            if (!Directory.Exists(dirPathString))
            {
                Directory.CreateDirectory(dirPathString);
            }

            //existing files should be overwritten
            using (StreamWriter file = File.CreateText(filePathString))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, item);
            }
        }

        private T ReadItemFromFile(string dir, string name)
        {
            T item;
            string pathString = Path.Combine(_mainPath, dir);
            string fileName = $"{name}.json";
            pathString = Path.Combine(pathString, fileName);

            using (StreamReader file = File.OpenText(pathString))
            {
                JsonSerializer serializer = new JsonSerializer();
                item = (T)serializer.Deserialize(file, typeof(T));
            }

            return item;
        }

        private IEnumerable<string> GetAllFiles(string dir)
        {
            string pathString = System.IO.Path.Combine(_mainPath, dir);
            DirectoryInfo di = new DirectoryInfo(pathString);

            if (di.Exists)
            {
                return di.GetFiles("*.json", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileNameWithoutExtension(x.Name));
            }
            return new List<string>();
        }

    }
}
