using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataTransformUtilities.Models
{
    [XmlRoot(ElementName = "start", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class Start
    {
        public double ele { get; set; }
        public DateTime time { get; set; }
        [XmlAttribute("lat")]
        public double lat { get; set; }
        [XmlAttribute("lon")]
        public double lon { get; set; }
    }

    [XmlRoot(ElementName = "trkpt", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class Trkpt
    {
        public double ele { get; set; }
        public DateTime time { get; set; }
        [XmlAttribute("lat")]
        public double lat { get; set; }
        [XmlAttribute("lon")]
        public double lon { get; set; }
    }

    [XmlRoot(ElementName = "trkseg", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class Trkseg
    {
        [XmlElement("start")]
        public List<Start> start { get; set; }
        [XmlElement("trkpt")]
        public List<Trkpt> trkpt { get; set; }
    }

    [XmlRoot(ElementName = "trk", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class Trk
    {
        public string name {get;set;}
        public Trkseg trkseg { get; set; }
    }

    [XmlRoot(ElementName = "wpt", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class Wpt
    {
        [XmlAttribute("lat")]
        public double lat { get; set; }
        [XmlAttribute("lon")]
        public double lon { get; set; }
        public double ele { get; set; }
        public string name { get; set; }
        public string cmt { get; set; }
        public string desc { get; set; }
        public string sym { get; set; }
    }

        [XmlRoot(ElementName = "gpx", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class Gpx
    {
        public Trk trk { get; set; }
        [XmlElement("wpt")]
        public List<Wpt> wpt { get; set; }
    }

}