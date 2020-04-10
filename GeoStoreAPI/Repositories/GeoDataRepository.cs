using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using GeoDataModels.Models;
using System.Linq;

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

        public string Create(GeoData incomingData, string userID)
        {
            var createData = new GeoData()
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

        public void Delete(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                _dataAccess.Delete(id);
            }
        }

        public IEnumerable<GeoData> GetAll(string userID, Func<GeoData, bool> filter)
        {
            Func<GeoData, bool> userFilter = (x) => x.UserID == userID;
            Func<GeoData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
            return _dataAccess.GetAll(combinedFilter);
        }

        public IEnumerable<GeoData> GetShared(string userID, Func<GeoData, bool> filter)
        {
            var data = new List<GeoData>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, x => true);

            foreach (var grant in grants)
            {
                Func<GeoData, bool> userFilter = (x) => x.UserID == grant.OwnerUserID;
                Func<GeoData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
                data.AddRange(_dataAccess.GetAll(combinedFilter));
            }

            return data;
        }

        public GeoData GetSingle(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        public void Update(string id, GeoData incomingData, string userID)
        {
            var existingData = _dataAccess.Get(id);
            if (existingData.UserID == userID)
            {
                var updateData = new GeoData()
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

        //todo: there has to be a better way to append data, this seems wrong
        //this is an ugly hack due to not being able to append to an existing collection.
        //can't append to the coordinate IEnumerable in the geometry, its readonly
        //instead I am creating new multiline feature, adding existing coordinates, then adding new ones.
        //then since i cant be bothered to figure out which one we were trying to append to
        //only the new multipoint will be added back to the feature collection
        //todo sooner rather than later: get rid of the bamcis dependency
        public void AppendMultiPointCollection(string id, IEnumerable<Coordinate> incomingCoords)
        {

            if (incomingCoords == null) return;

            var coordsCombined = new List<Position>();
            var existingGeoData = _dataAccess.Get(id);

            var existingMultiPoint = existingGeoData.Data.Features.Where(x => x.Geometry is BAMCIS.GeoJSON.MultiPoint).FirstOrDefault();
            if (existingMultiPoint != null)
            {
                var geometry = (BAMCIS.GeoJSON.MultiPoint)existingMultiPoint.Geometry;
                coordsCombined.AddRange(geometry.Coordinates.Select(x => new Position(x.Longitude, x.Latitude,x.Elevation)));                
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

        private IEnumerable<GeoData> GetMockData()
        {
            var testData = new List<GeoData>();
            var data1 = new GeoData()
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