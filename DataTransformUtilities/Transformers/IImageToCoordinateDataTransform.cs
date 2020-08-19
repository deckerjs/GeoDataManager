using CoordinateDataModels;
using System.IO;

namespace DataTransformUtilities.Transformers
{
    public interface IImageToCoordinateDataTransform
    {
        CoordinateData GetCoordinateData(Stream imageStream);
        PointCollection GetPointCollection(Stream imageStream);
    }
}