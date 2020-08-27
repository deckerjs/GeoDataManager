using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CoordinateDataModels;
using DataTransformUtilities.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace DataTransformUtilities.Transformers
{
    public class ImageToCoordinateDataTransform : IImageToCoordinateDataTransform
    {

        public CoordinateData GetCoordinateData(Stream imageStream)
        {
            var coordinateData = new CoordinateData();
            string dataDescription = "";

            coordinateData.UserID = "";
            coordinateData.ID = Guid.NewGuid().ToString();
            coordinateData.DateCreated = DateTime.Now;
            coordinateData.Tags = new List<string> { "Exif Data Source" };
            coordinateData.DateModified = DateTime.Now;

            coordinateData.Description = dataDescription;
            coordinateData.Data = new List<PointCollection>() { GetPointCollection(imageStream) };
            return coordinateData;
        }

        public PointCollection GetPointCollection(Stream imageStream)
        {
            var pc = new PointCollection();

            using (imageStream)
            {
                using (var image = new Bitmap(imageStream))
                {
                    // lat,lon,alt
                    if (Array.IndexOf<int>(image.PropertyIdList, 1) != -1 &&
                       Array.IndexOf<int>(image.PropertyIdList, 2) != -1 &&
                       Array.IndexOf<int>(image.PropertyIdList, 3) != -1 &&
                       Array.IndexOf<int>(image.PropertyIdList, 4) != -1 &&
                       Array.IndexOf<int>(image.PropertyIdList, 6) != -1)
                    {
                        string gpsLatitudeRef = BitConverter.ToChar(image.GetPropertyItem(1).Value, 0).ToString();
                        DegMinSec latitude = GetDegMinSecFromImageProp(image.GetPropertyItem(2));
                        double? lat = GetDoubleFromDegMinSec(GetDegMinSecFromImageProp(image.GetPropertyItem(2)), gpsLatitudeRef.Equals("S", StringComparison.OrdinalIgnoreCase));

                        string gpsLongitudeRef = BitConverter.ToChar(image.GetPropertyItem(3).Value, 0).ToString();
                        DegMinSec longitude = GetDegMinSecFromImageProp(image.GetPropertyItem(4));
                        double? lon = GetDoubleFromDegMinSec(GetDegMinSecFromImageProp(image.GetPropertyItem(4)), gpsLongitudeRef.Equals("W", StringComparison.OrdinalIgnoreCase));

                        string gpsAltitudeRef = "";// BitConverter.ToChar(image.GetPropertyItem(5).Value, 0).ToString();
                        double gpsAltitude = GetDoubleFromRational(image.GetPropertyItem(6).Value);

                        Console.WriteLine($"lat: {lat} lon: {lon}");
                        Console.WriteLine($"altitude ref: {gpsAltitudeRef} altitude: {gpsAltitude}");

                        DateTime? imgDate = null;
                        if (Array.IndexOf<int>(image.PropertyIdList, 0x0132) != -1)
                        {
                            Regex rgx = new Regex(":");
                            string imgDateStr = rgx.Replace(Encoding.UTF8.GetString(image.GetPropertyItem(0x0132).Value), "-", 2);
                            imgDate = DateTime.Parse(imgDateStr);
                        }

                        Dictionary<string, double> telemetry = new Dictionary<string, double>();
                        //measure mode
                        int gpsMeasureRef;
                        if (Array.IndexOf<int>(image.PropertyIdList, 0x000A) != -1)
                        {
                            gpsMeasureRef = BitConverter.ToChar(image.GetPropertyItem(0x000A).Value, 0);
                            telemetry.Add("GpsMeasureRef", gpsMeasureRef);
                        }

                        //DOP 2 - HDOP, 3 - PDOP
                        double gpsDOP;
                        if (Array.IndexOf<int>(image.PropertyIdList, 0x000B) != -1)
                        {
                            gpsDOP = GetDoubleFromRational(image.GetPropertyItem(0x000B).Value);
                            telemetry.Add("GpsDOP", gpsDOP);
                        }

                        if (lat != null && lon != null)
                        {
                            pc.Coordinates.Add(new Coordinate(lat.GetValueOrDefault(), lon.GetValueOrDefault(), gpsAltitude, imgDate, telemetry));
                        }
                    }
                    pc.ID = Guid.NewGuid().ToString();
                    return pc;

                }
            }

        }
        private DegMinSec GetDegMinSecFromImageProp(System.Drawing.Imaging.PropertyItem propertyItem)
        {
            uint degN = BitConverter.ToUInt32(propertyItem.Value, 0);
            uint degD = BitConverter.ToUInt32(propertyItem.Value, 4);
            uint minN = BitConverter.ToUInt32(propertyItem.Value, 8);
            uint minD = BitConverter.ToUInt32(propertyItem.Value, 12);
            uint secN = BitConverter.ToUInt32(propertyItem.Value, 16);
            uint secD = BitConverter.ToUInt32(propertyItem.Value, 20);

            DegMinSec degMinSec = new DegMinSec();

            if (degD > 0) { degMinSec.Deg = (double)degN / degD; } else { degMinSec.Deg = degN; }
            if (minD > 0) { degMinSec.Min = (double)minN / minD; } else { degMinSec.Min = minN; }
            if (secD > 0) { degMinSec.Sec = (double)secN / secD; } else { degMinSec.Sec = secN; }

            return degMinSec;
        }

        private double GetDoubleFromRational(byte[] rational)
        {
            double result = 0;
            uint numerator = BitConverter.ToUInt32(rational, 0);
            uint denominator = BitConverter.ToUInt32(rational, 4);
            if (denominator > 0) { result = (double)numerator / denominator; } else { result = numerator; }
            return result;
        }

        private double? GetDoubleFromDegMinSec(DegMinSec degMinSec, bool isNegative)
        {
            var value = Math.Abs(degMinSec.Deg) + degMinSec.Min / 60.0d + degMinSec.Sec / 3600.0d;
            if (double.IsNaN(value))
                return null;
            if (isNegative)
                value *= -1;
            return value;
        }

        private class DegMinSec
        {
            public double Deg;
            public double Min;
            public double Sec;
        }

    }
}
