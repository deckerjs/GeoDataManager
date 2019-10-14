using System.Collections.Generic;
using BAMCIS.GeoJSON;

namespace GeoDataModels.Models
{
    public class MultiPoint : BAMCIS.GeoJSON.MultiPoint
    {
        public MultiPoint(
            IEnumerable<Position> coordinates,
            IEnumerable<double> boundingBox = null) : base(coordinates, boundingBox)
        {
        }
    }
}