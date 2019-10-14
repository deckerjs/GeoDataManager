namespace GeoDataModels.Models
{

    public class Position : BAMCIS.GeoJSON.Position
    {
        public Position(double longitude, double latitude) : base(longitude, latitude)
        {
        }

        public Position(double longitude, double latitude, double elevation) : base(longitude, latitude, elevation)
        {
        }
    }

}