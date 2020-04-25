using System;
using GeoDataModels.Models;
using DataTransformUtilities.Models;
using System.Collections.Generic;
using System.Linq;
using CoordinateDataModels;

namespace DataTransformUtilities.Transformers
{
    public interface IGPXTransform<T>
    {
        T GetDataFromGpx(Gpx gpx);        
    }
    
}
