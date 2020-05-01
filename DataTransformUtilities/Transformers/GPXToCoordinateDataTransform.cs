﻿using CoordinateDataModels;
using DataTransformUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransformUtilities.Transformers
{
    public class GPXToCoordinateDataTransform : IGPXTransform<CoordinateData>
    {
        public CoordinateData GetDataFromGpx(Gpx gpx)
        {
            var coordinateData = new CoordinateData();
            string dataDescription = "";

            coordinateData.UserID = "";
            coordinateData.ID = Guid.NewGuid().ToString();
            coordinateData.DateCreated = DateTime.Now;
            coordinateData.Tags = new List<string> { "Transformed From GPX" };
            coordinateData.DateModified = DateTime.Now;

            var pointCollections = new List<PointCollection>();

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

                pointCollections.AddRange(GetPointCollectionFromTrk(gpx.trk));
            }

            if (gpx.wpt != null && gpx.wpt.Any())
            {
                dataDescription = string.Concat(dataDescription, "Waypoints: ", gpx.wpt.Count());
                pointCollections.AddRange(GetWptFeatures(gpx.wpt));
            }

            coordinateData.Description = dataDescription;
            coordinateData.Data = pointCollections;
            return coordinateData;
        }

        private List<PointCollection> GetPointCollectionFromTrk(Trk trk)
        {
            var pointCollections = new List<PointCollection>();
            var coords = new List<Coordinate>();

            foreach (var coord in trk.trkseg.trkpt)
            {
                coords.Add(new Coordinate(coord.lon, coord.lat, coord.ele, coord.time));
            }

            var props = new Dictionary<string, string>();
            props["Name"] = trk.name;
            props["StartTime"] = coords.First().Time.ToString();
            props["EndtTime"] = coords.Last().Time.ToString();
  
            var pointCollection = new PointCollection()
            {
                Coordinates = coords,
                ID = Guid.NewGuid().ToString(),
                Metadata = props
            };

            pointCollections.Add(pointCollection);

            return pointCollections;
        }

        private List<PointCollection> GetWptFeatures(List<Wpt> wpts)
        {
            var pointCollections = new List<PointCollection>();                        
            foreach (var wpt in wpts)
            {
                var coords = new List<Coordinate>();
                coords.Add(new Coordinate(wpt.lon, wpt.lat, wpt.ele, null));

                var props = new Dictionary<string, string>();
                props["Name"] = wpt.name;
                props["Cmt"] = wpt.cmt;
                props["Desc"] = wpt.desc;
                props["Sym"] = wpt.sym;

                var pointCollection = new PointCollection()
                {
                    Coordinates = coords,
                    ID = Guid.NewGuid().ToString(),
                    Metadata = props
                };

                pointCollections.Add(pointCollection);
            }
            return pointCollections;
        }
    }

}