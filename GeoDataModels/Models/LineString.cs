using System.Collections.Generic;
using BAMCIS.GeoJSON;

namespace GeoDataModels.Models
{
    public class LineString : BAMCIS.GeoJSON.LineString
    {
        public LineString(
            IEnumerable<Position> coordinates,
            IEnumerable<double> boundingBox = null) : base(coordinates, boundingBox)
        {
        }
    }
}