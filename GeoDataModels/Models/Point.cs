using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataModels.Models
{
    public class Point : BAMCIS.GeoJSON.Point
    {
        public Point(BAMCIS.GeoJSON.Position coordinates,
                     IEnumerable<double> boundingBox = null) : base(coordinates, boundingBox)
        {
        }
    }
}
