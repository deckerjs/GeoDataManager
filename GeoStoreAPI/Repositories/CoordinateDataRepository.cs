using System;
using System.Collections.Generic;
using GeoStoreAPI.DataAccess;
using System.Linq;
using CoordinateDataModels;

namespace GeoStoreAPI.Repositories
{
    public class CoordinateDataRepository : ICoordinateDataRepository
    {
        private readonly ICoordinateDataAccess _dataAccess;
        private readonly IUserDataPermissionRepository _dataPermissionRepository;

        public CoordinateDataRepository(ICoordinateDataAccess dataAccess,
            IUserDataPermissionRepository dataPermissionRepository)
        {
            _dataAccess = dataAccess;
            _dataPermissionRepository = dataPermissionRepository;
        }

        public string Create(CoordinateData incomingData, string userID)
        {
            var createData = new CoordinateData()
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

        public IEnumerable<CoordinateData> GetAll(string userID, Func<CoordinateData, bool> filter)
        {
            Func<CoordinateData, bool> userFilter = (x) => x.UserID == userID;
            Func<CoordinateData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
            return _dataAccess.GetAll(combinedFilter);
        }

        public IEnumerable<CoordinateData> GetShared(string userID, Func<CoordinateData, bool> filter)
        {
            var data = new List<CoordinateData>();
            var grants = _dataPermissionRepository.GetAllGrantedToUser(userID, x => true);

            foreach (var grant in grants)
            {
                Func<CoordinateData, bool> userFilter = (x) => x.UserID == grant.OwnerUserID;
                Func<CoordinateData, bool> combinedFilter = (x) => filter(x) && userFilter(x);
                data.AddRange(_dataAccess.GetAll(combinedFilter));
            }

            return data;
        }

        public CoordinateData GetSingle(string id, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                return data;
            }
            return null;
        }

        public void Update(string id, CoordinateData incomingData, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                var updateData = new CoordinateData()
                {
                    ID = data.ID,
                    UserID = data.UserID,
                    DateCreated = data.DateCreated,
                    DateModified = DateTime.Now,
                    Description = incomingData.Description,
                    Tags = incomingData.Tags,
                    Data = incomingData.Data
                };
                _dataAccess.Update(id, updateData);
            }
        }

        public void AddPointCollection(string id, IEnumerable<Coordinate> coordinates, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                int nextId = data.Data.Any() ? data.Data.Count() + 1 : 1;
                var newPc = new PointCollection
                {
                    ID = nextId.ToString(),
                    Coordinates = coordinates.ToList()
                };
                data.Data.Add(newPc);
                _dataAccess.Update(id, data);
            }
        }

        public void AppendToPointCollection(string id, string pcid, IEnumerable<Coordinate> coordinates, string userID)
        {
            var data = _dataAccess.Get(id);
            if (data != null && data.UserID == userID)
            {
                var pc = data.Data.Where(x => x.ID == pcid).FirstOrDefault();
                if (pc != null)
                {
                    pc.Coordinates.AddRange(coordinates);
                }
                else
                {
                    var newPc = new PointCollection()
                    {
                        ID = pcid,
                        Coordinates = coordinates.ToList()
                    };
                    data.Data.Add(newPc);
                }

                _dataAccess.Update(id, data);
            }
        }

    }
}