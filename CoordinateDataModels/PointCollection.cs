using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoordinateDataModels
{
    public class PointCollection
    {
        public PointCollection()
        {
            Metadata = new Dictionary<string, string>();
            Coordinates = new List<Coordinate>();
        }
        [Required]
        public string ID { get; set; }
        public Dictionary<string,string> Metadata { get; set; }
        public List<Coordinate> Coordinates { get; set; }
    }
}
