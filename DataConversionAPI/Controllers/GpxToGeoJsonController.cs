using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using DataTransformUtilities.Models;
using GeoDataModels.Models;
using DataTransformUtilities.Transformers;

namespace DataConversionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpxToGeoJsonController : ControllerBase
    {
        private readonly IGPXTransform<GeoJsonData> _gpxTransform;

        public GpxToGeoJsonController(IGPXTransform<GeoJsonData> gpxTransform)
        {
            _gpxTransform = gpxTransform;
        }

        [HttpPost("FromXML")]
        public GeoJsonData PostAsXML([FromBody] Gpx gpx)
        {            
            var geoData = _gpxTransform.GetDataFromGpx(gpx);
            return geoData;
        }

        [HttpPost("FromText")]
        public GeoJsonData PostAsText()
        {
            string plainText;
            using (var reader = new StreamReader(Request.Body))
            {
                plainText = reader.ReadToEnd();
                Console.WriteLine(plainText);
            }

            using (MemoryStream stream = new MemoryStream(StringToUTF8ByteArray(plainText)))
            {
                var gpx = (Gpx)new XmlSerializer(typeof(Gpx), "http://www.topografix.com/GPX/1/1").Deserialize(stream);
                var geoData = _gpxTransform.GetDataFromGpx(gpx);
                return geoData;
            }
        }

        private static byte[] StringToUTF8ByteArray(string xml)
        {
            return new UTF8Encoding().GetBytes(xml);
        }

    }

}
