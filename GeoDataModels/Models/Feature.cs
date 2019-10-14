using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BAMCIS.GeoJSON;
using Newtonsoft.Json;

namespace GeoDataModels.Models
{
    public class Feature : BAMCIS.GeoJSON.Feature
    {
        public Feature(
            Geometry geometry,
            IDictionary<string, dynamic> properties = null,
            IEnumerable<double> boundingBox = null) : base(geometry, properties, boundingBox)
        {
        }
    }
}