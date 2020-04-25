using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Models
{
    public abstract class CoordinateDataBase<T>
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<string> Tags { get; set; }
        public T Data { get; set; }
    }
}
