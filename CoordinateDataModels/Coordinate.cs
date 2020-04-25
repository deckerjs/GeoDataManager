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

        public double Latitude { get; }
        public double Longitude { get; }
        public double Elevation { get; }
        public DateTime? Time { get; }
    }
}
