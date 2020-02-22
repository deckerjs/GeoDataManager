using System;
using GeoDataModels.Models;
using DataTransformUtilities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataTransformUtilities.Transformers
{
    public class GPXTransform : IGPXTransform
    {

        public GeoData GetGeoDataFromGpx(Gpx gpx)
        {
            var geoData = new GeoData()
            {
                UserID = "",
                ID = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                Description = gpx.trk.name,
                Tags = { "Transformed From GPX" },
                DateModified = DateTime.Now,
                Data = GetFeatureCollection(gpx)
            };

            return geoData;
        }
        private FeatureCollection GetFeatureCollection(Gpx gpx)
        {
            List<Position> coords = new List<Position>();
            List<DateTime> coordTimes = new List<DateTime>();

            foreach (var coord in gpx.trk.trkseg.trkpt)
            {
                coords.Add(new Position(coord.lon, coord.lat, coord.ele));
                coordTimes.Add(coord.time);
            }
            
            var geom1 = new LineString(coords);

            var props = new Dictionary<string, object>();
            props["Name"] = gpx.trk.name;
            props["StartTime"] = coordTimes.First();
            props["EndtTime"] = coordTimes.Last();
            props["CoordinateTimes"] = coordTimes;

            var feature1 = new Feature(geom1, props);

            var features = new List<Feature>();
            features.Add(feature1);

            var data = new FeatureCollection(features);

            return data;
        }


    }
}
