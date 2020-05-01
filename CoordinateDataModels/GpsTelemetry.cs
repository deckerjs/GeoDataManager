using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateDataModels
{
    public class GpsTelemetry
    {
        public double Quality { get; set; }
        public double Heading { get; set; }
        public double FeetPerSecond { get; set; }
        public int SatellitesInView { get; set; }
        public double SignalToNoiseRatio { get; set; }
        public double RtkAge { get; set; }
        public double RtkRatio { get; set; }
        public double Hdop { get; set; }
        public double EastProjectionOfBaseLine { get; set; }
        public double NorthProjectionOfBaseLine { get; set; }
        public double UpProjectionOfBaseLine { get; set; }

    }
}
