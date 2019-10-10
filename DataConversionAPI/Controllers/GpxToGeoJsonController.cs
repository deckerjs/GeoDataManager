using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DataConversionAPI.Models;
using DataConversionAPI.Services;
using GeoStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataConversionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpxToGeoJsonController : ControllerBase
    {
        private readonly GPXTransformer _gpxTransformer;

        public GpxToGeoJsonController(GPXTransformer gpxTransformer)
        {
            _gpxTransformer = gpxTransformer;
        }

        [HttpPost("FromXML")]
        public GeoData PostAsXML([FromBody] Gpx gpx)
        {            
            var geoData = _gpxTransformer.GetGeoDataFromGpx(gpx);
            return geoData;
        }

        [HttpPost("FromText")]
        public GeoData PostAsText()
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
                var geoData = _gpxTransformer.GetGeoDataFromGpx(gpx);
                return geoData;
            }
        }

        private static byte[] StringToUTF8ByteArray(string xml)
        {
            return new UTF8Encoding().GetBytes(xml);
        }

    }

}
