using System;
using System.Collections.Generic;

namespace GeoDataModels.Models
{
    public class GeoData
    {
        public GeoData()
        {
            Tags = new List<string>();
            Data = new FeatureCollection();
        }

        public string ID { get; set; }
        public string UserID{get;set;}
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public List<string> Tags { get; set; }
        public FeatureCollection Data {get;set;}
    }
}