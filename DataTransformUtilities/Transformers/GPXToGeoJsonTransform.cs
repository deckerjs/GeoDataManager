using System;
using GeoDataModels.Models;
using DataTransformUtilities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataTransformUtilities.Transformers
{
    public class GPXToGeoJsonTransform : IGPXTransform<GeoJsonData>
    {

        public GeoJsonData GetDataFromGpx(Gpx gpx)
        {
            var geoData = new GeoJsonData();
            string dataDescription = "";

            geoData.UserID = "";
            geoData.ID = Guid.NewGuid().ToString();
            geoData.DateCreated = DateTime.Now;
            geoData.Tags = new List<string> { "Transformed From GPX" };
            geoData.DateModified = DateTime.Now;

            var features = new List<Feature>();

            if (gpx.trk != null)
            {
                if (!string.IsNullOrEmpty(gpx.trk.name))
                {
                    dataDescription = string.Concat(dataDescription, gpx.trk.name);
                }
                else
                {
                    dataDescription = string.Concat(dataDescription, "Un-named track");
                }

                features.AddRange(GetTrkFeatures(gpx.trk));
            }

            if (gpx.wpt != null && gpx.wpt.Any())
            {
                dataDescription = string.Concat(dataDescription, "Waypoints: ", gpx.wpt.Count());
                features.AddRange(GetWptFeatures(gpx.wpt));
            }

            geoData.Description = dataDescription;
            geoData.Data = new FeatureCollection(features);
            return geoData;
        }

        private List<Feature> GetTrkFeatures(Trk trk)
        {
            List<Position> coords = new List<Position>();
            List<DateTime> coordTimes = new List<DateTime>();
            var features = new List<Feature>();

            foreach (var trkseg in trk.trkseg)
            {
                foreach (var coord in trkseg.trkpt)
                {
                    coords.Add(new Position(coord.lon, coord.lat, coord.ele));
                    coordTimes.Add(coord.time);
                }


                BAMCIS.GeoJSON.Geometry geom1;
                if (coords.Count > 1)
                {
                    geom1 = new LineString(coords);
                }
                else
                {
                    geom1 = new Point(coords.First());
                }

                var props = new Dictionary<string, object>();
                props["Name"] = trk.name;
                props["StartTime"] = coordTimes.First();
                props["EndtTime"] = coordTimes.Last();
                props["CoordinateTimes"] = coordTimes;

                var feature1 = new Feature(geom1, props);
                
                features.Add(feature1);
            }
            return features;
        }

        private List<Feature> GetWptFeatures(List<Wpt> wpts)
        {
            var features = new List<Feature>();
            foreach (var wpt in wpts)
            {
                var pos = new Position(wpt.lon, wpt.lat, wpt.ele);
                var props = new Dictionary<string, object>();
                props["Name"] = wpt.name;
                props["Cmt"] = wpt.cmt;
                props["Desc"] = wpt.desc;
                props["Sym"] = wpt.sym;

                var point = new Point(pos);
                var feature = new Feature(point, props);

                features.Add(feature);
            }
            return features;
        }
    }
}
