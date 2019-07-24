using System.Collections.Generic;
using BAMCIS.GeoJSON;

namespace GeoStoreAPI.Models
{
    public class FeatureCollection: BAMCIS.GeoJSON.FeatureCollection
    {
        public FeatureCollection():this(new List<Feature>())
        {
        }

        public FeatureCollection(IEnumerable<Feature> features):base(features)
        {
        }
    }
}