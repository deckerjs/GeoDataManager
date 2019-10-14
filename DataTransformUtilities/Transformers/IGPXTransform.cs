using System;
using GeoDataModels.Models;
using DataTransformUtilities.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataTransformUtilities.Transformers
{
    public interface IGPXTransform
    {
        GeoData GetGeoDataFromGpx(Gpx gpx);
    }
    
}
