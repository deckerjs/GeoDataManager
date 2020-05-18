using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using GeoDataModels.Models;
using System.Linq;
using GeoStoreAPI.Models;
using System.Linq.Expressions;
using GeoStoreAPI.Services;

namespace GeoStoreAPI.Repositories
{
    public class GeoDataRepository : IGeoDataRepository
    {
        private readonly IGeoDataAccess _dataAccess;
        private readonly IUserDataPermissionRepository _dataPermissionRepository;

        public GeoDataRepository(IGeoDataAccess dataAccess,
            IUserDataPermissionRepository dataPermissionRepository)
        {
            _dataAccess = dataAccess;
            _dataPermissionRepository = dataPermissionRepository;
        }

        [Obsolete]
        public string Create(GeoJsonData incomingData, string userID)
        {
            var createData = new GeoJsonData()
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Description = incomingData.Description,
                Tags = incomingData.Tags,
                Data = incomingData.Data
            };

            _dataAccess.Create(createData);
            return createData.ID;
        }

        [Obsolete]
        public void Delete(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                _dataAccess.Delete(id);
            }
        }

        public IEnumerable<GeoJsonData> GetAll(string userID, IEnumerable<Expression<Func<GeoJsonData, bool>>> filter)
        {
            Expression<Func<GeoJsonData, bool>> userFilter = FilterExpressionUtilities.GetEqExpressionForProperty<GeoJsonData>("UserID", userID);
            var filters = filter.ToList();
            filters.Add(userFilter);
            return _dataAccess.GetAll(filters);
        }

        public IEnumerable<GeoJsonData> GetShared(string userID, IEnumerable<Expression<Func<GeoJsonData, bool>>> filter)
        {
            var data = new List<GeoJsonData>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, null);
                        
            foreach (var grant in grants)
            {
                Expression<Func<GeoJsonData, bool>> userFilter = FilterExpressionUtilities.GetEqExpressionForProperty<GeoJsonData>("UserID", grant.OwnerUserID);
                var filters = filter.ToList();
                filters.Add(userFilter);
                data.AddRange(_dataAccess.GetAll(filters));
            }

            return data;
        }

        public GeoJsonData GetSingle(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        [Obsolete]
        public void Update(string id, GeoJsonData incomingData, string userID)
        {
            var existingData = _dataAccess.Get(id);
            if (existingData.UserID == userID)
            {
                var updateData = new GeoJsonData()
                {
                    ID = existingData.ID,
                    UserID = existingData.UserID,
                    DateCreated = existingData.DateCreated,
                    DateModified = DateTime.Now,
                    Description = incomingData.Description,
                    Tags = incomingData.Tags,
                    Data = incomingData.Data
                };

                _dataAccess.Update(id, updateData);
            }
        }

        [Obsolete]
        public void AppendMultiPointCollection(string id, IEnumerable<Coordinate> incomingCoords)
        {

            if (incomingCoords == null) return;

            var coordsCombined = new List<Position>();
            var existingGeoData = _dataAccess.Get(id);

            var existingMultiPoint = existingGeoData.Data.Features.Where(x => x.Geometry is BAMCIS.GeoJSON.MultiPoint).FirstOrDefault();
            if (existingMultiPoint != null)
            {
                var geometry = (BAMCIS.GeoJSON.MultiPoint)existingMultiPoint.Geometry;
                coordsCombined.AddRange(geometry.Coordinates.Select(x => new Position(x.Longitude, x.Latitude, x.Elevation)));
            }

            coordsCombined.AddRange(GetPositionsFromCoordinates(incomingCoords));

            var multiPoint = new MultiPoint(coordsCombined);
            var features = new List<Feature>();
            features.Add(new Feature(multiPoint));
            var appendedData = new FeatureCollection(features);

            //this will wipe out any other features if there were any. hooray for read only collection properties
            existingGeoData.Data = appendedData;
            existingGeoData.DateModified = DateTime.Now;

            _dataAccess.Update(existingGeoData.ID, existingGeoData);
        }

        private List<Position> GetPositionsFromCoordinates(IEnumerable<Coordinate> coords)
        {
            return new List<Position>(coords.Select(x => new Position(x.Longitude, x.Latitude, x.Elevation)));
        }

        [Obsolete]
        public List<Coordinate> GetCoordinatesFromFeatureCollection(FeatureCollection featureCollection)
        {
            var existingMultiPoint = featureCollection.Features.Where(x => x.Geometry is BAMCIS.GeoJSON.MultiPoint).FirstOrDefault();
            if (existingMultiPoint != null)
            {
                var geometry = (BAMCIS.GeoJSON.MultiPoint)existingMultiPoint.Geometry;
                return geometry.Coordinates.Select(x => new Coordinate() { Latitude = x.Latitude, Longitude = x.Longitude, Elevation = x.Elevation }).ToList();
            }
            return null;
        }

        private MultiPoint GetMultiPointFromCoordinates(IEnumerable<Coordinate> coords)
        {
            return new MultiPoint(coords.Select(x => new Position(x.Longitude, x.Latitude, x.Elevation)));
        }

        private FeatureCollection GetCoordinatesFeatureCollection(IEnumerable<Coordinate> coords)
        {
            var geom = new MultiPoint(coords.Select(x => new Position(x.Longitude, x.Latitude, x.Elevation)));
            var feature = new Feature(geom);
            var features = new List<Feature>();

            features.Add(feature);
            return new FeatureCollection(features);
        }

        private IEnumerable<GeoJsonData> GetMockData()
        {
            var testData = new List<GeoJsonData>();
            var data1 = new GeoJsonData()
            {
                UserID = "testuser1234",
                ID = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                Description = "test description",
                Tags = { "some tag", "some other tag", "tag 3" },
                DateModified = DateTime.Now,
                Data = GetSampleFeatureCollection()
            };
            testData.Add(data1);
            return testData;
        }

        private FeatureCollection GetSampleFeatureCollection()
        {
            var coords = new List<Position>(){
                 new Position(-88.095398000000003, 42.918624000000001, 252.93600000000001),
                 new Position(-88.095395999999994, 42.918604000000002, 245.953),
                 new Position(-88.095110000000005, 42.918570000000003, 241.78899999999999)
             };

            var geom1 = new MultiPoint(coords);

            var props = new Dictionary<string, object>();
            props["Name"] = "prop 1 name";

            var feature1 = new Feature(geom1, props);

            var features = new List<Feature>();
            features.Add(feature1);

            var data = new FeatureCollection(features);

            return data;
        }

    }
}