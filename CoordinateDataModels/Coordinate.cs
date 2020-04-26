using System;

namespace CoordinateDataModels
{
    public class Coordinate
    {
        public Coordinate() { }
        public Coordinate(double latitude, double longitude, double elevation, DateTime? time)
        {
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
            Time = time;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime? Time { get; set; }
    }
}
