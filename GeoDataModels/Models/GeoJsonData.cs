using CoordinateDataModels;
using System;
using System.Collections.Generic;

namespace GeoDataModels.Models
{

    public class GeoJsonData : CoordinateDataAbstractBase<FeatureCollection>
    {
    }

    //public class GeoJsonData
    //{
    //    public GeoJsonData()
    //    {
    //        Tags = new List<string>();
    //        Data = new FeatureCollection();

    //        //replace feature colllection with own points collection type
    //        // when geojson is needed convert points to feature collection as is done already 
    //        // in the mock data gen, and points insert
    //        // geojson is never edited or saved.
    //        // new storage type:
    //        //  - collection of position (lat,lon,ele,time)
    //        //  - metadata dictionary for anything else
    //        //take into account, 
    //        // multiple tracks per gpx file
    //        // waypoints instead of tracks

    //        // transformer idea ** messy
    //        // leave everything as is except for storage
    //        // new type only used in repo/dataaccess
    //        //convert to new type in repo before saving, and back to geojson when loading
    //        //wouldmake filtering messy

    //        //geojson get-only endpoint ** clean
    //        //normal classes everywhere, weird geojson only for maps, or whatever
    //        //minor existing controller changes
    //        // remove post/put (possibly only temporary)
    //        // new controller for track/point crud
    //        //repo changes for geojson
    //        // read normal data, and generate featurelist

    //    }

    //    public string ID { get; set; }
    //    public string UserID { get; set; }
    //    public string Description { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public DateTime DateModified { get; set; }
    //    public List<string> Tags { get; set; }
    //    public FeatureCollection Data { get; set; }
    //}
}