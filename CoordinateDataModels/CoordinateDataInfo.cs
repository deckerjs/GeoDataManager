using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateDataModels
{
    public class CoordinateDataInfo
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<string> Tags { get; set; }
    }
}
