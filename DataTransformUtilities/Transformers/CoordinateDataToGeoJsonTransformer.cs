using CoordinateDataModels;
using GeoDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransformUtilities.Transformers
{
    public class CoordinateDataToGeoJsonTransformer
    {
        public static GeoJsonData GetGeoJsonFromCoordinateData(CoordinateData coordinateData)
        {
            return GetDataFromCoordinateData(coordinateData);
        }

        public static GeoJsonData GetDataFromCoordinateData(CoordinateData coordinateData)
        {
            var geoData = new GeoJsonData();
            string dataDescription = "";

            geoData.UserID = coordinateData.UserID;
            geoData.ID = coordinateData.ID;
            geoData.DateCreated = coordinateData.DateCreated;
            geoData.Tags = coordinateData.Tags;
            geoData.DateModified = coordinateData.DateModified;

            var features = new List<Feature>();

            if (coordinateData.Data != null)
            {                
                dataDescription = coordinateData.Description;

                features.AddRange(GetTrkFeatures(coordinateData.Data));
            }
            
            geoData.Description = dataDescription;
            geoData.Data = new FeatureCollection(features);
            return geoData;
        }

        private static List<Feature> GetTrkFeatures(List<PointCollection> data)
        {
            var features = new List<Feature>();

            foreach (var pointCollection in data)
            {

                List<Position> coords = new List<Position>();
                List<DateTime> coordTimes = new List<DateTime>();

                foreach (var coord in pointCollection.Coordinates)
                {
                    coords.Add(new Position(coord.Longitude, coord.Latitude, coord.Altitude));
                    coordTimes.Add(coord.Time.GetValueOrDefault());
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

                foreach (var metaDataItem in pointCollection.Metadata)
                {
                    props.Add(metaDataItem.Key, metaDataItem.Value);
                }
                       
                var feature1 = new Feature(geom1, props);
                features.Add(feature1);
            }

            return features;
        }
        

    }
}
