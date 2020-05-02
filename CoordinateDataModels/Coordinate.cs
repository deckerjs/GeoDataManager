using System;
using System.Collections.Generic;

namespace CoordinateDataModels
{
    public class Coordinate
    {
        public Coordinate()
        {
            Telemetry = new GpsTelemetry();
        }

        public Coordinate(double latitude, double longitude, double altitude, DateTime? time) : this(latitude, longitude, altitude, time, null) {}

        public Coordinate(double latitude, double longitude, double altitude, DateTime? time, GpsTelemetry telemetry)
        {
            Telemetry = new GpsTelemetry();
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            Time = time;

            if (telemetry != null)
            {
                Telemetry = telemetry;
            }
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public DateTime? Time { get; set; }

        public GpsTelemetry Telemetry { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }
}
