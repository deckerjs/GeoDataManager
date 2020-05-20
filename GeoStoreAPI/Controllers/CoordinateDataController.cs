using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoordinateDataModels;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoordinateDataController : ControllerBase
    {
        private readonly ICoordinateDataRepository _dataRepository;
        private readonly IUserIdentificationService _userIdService;
        private readonly IQueryStringFilterBuilderService _filterBuilder;

        public CoordinateDataController(
            ICoordinateDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IQueryStringFilterBuilderService filterBuilder)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
            _filterBuilder = filterBuilder;
        }

        /// <summary>
        /// Get all CoordinateData for current user
        /// </summary>
        /// <returns>List of CoordinateData</returns>
        [HttpGet]
        public ActionResult<IEnumerable<CoordinateData>> GetAll()
        {
            var filter = _filterBuilder.GetFilter<CoordinateData>();
            var result = _dataRepository.GetAll(_userIdService.GetUserID(), filter);
            if (result != null) return result.ToList();
            return null;
        }

        /// <summary>
        /// Get lightweight version of coordinatedata
        /// </summary>
        /// <returns>List of CoordinateDataInfo</returns>
        [HttpGet]
        public ActionResult<IEnumerable<CoordinateDataInfo>> GetSummary()
        {
            var filter = _filterBuilder.GetFilter<CoordinateDataInfo>();
            var result = _dataRepository.GetSummary(_userIdService.GetUserID(), filter);
            if (result != null) return result.ToList();
            return null;
        }

        /// <summary>
        /// Gets all CoordinateData items shared to current user
        /// Basic (experimental) querystring property filter available
        /// ex: ?UserID='ABC123'
        /// </summary>
        /// <returns>List of CoordinateData</returns>
        [HttpGet]
        [Route("Shared")]
        public ActionResult<IEnumerable<CoordinateData>> GetShared()
        {
            var filter = _filterBuilder.GetFilter<CoordinateData>();
            return _dataRepository.GetShared(_userIdService.GetUserID(), filter).ToList();
        }

        /// <summary>
        /// Get CoordinateData
        /// </summary>
        /// <param name="id">CoordinateData.ID</param>
        /// <returns>CoordinateData</returns>
        [HttpGet("{id}")]
        public ActionResult<CoordinateData> Get(string id)
        {
            var data = _dataRepository.GetSingle(id, _userIdService.GetUserID());
            if (data != null)
            {
                return data;
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create CoordinateData
        /// </summary>
        /// <param name="coordinateData">CoordinateData</param>
        /// <returns>CoordinateData.ID</returns>
        [HttpPost]
        public string Post([FromBody] CoordinateData coordinateData)
        {
            return _dataRepository.Create(coordinateData, _userIdService.GetUserID());
        }

        /// <summary>
        /// Append points to an existing PointCollection if it exists, 
        /// otherwise it is created with the provided PointCollection id
        /// </summary>
        /// <param name="id">CoordinateData.ID</param>
        /// <param name="pcid">PointCollection.ID</param>
        /// <param name="coordinates">List of {lat,lon,ele,time}</param>
        /// <returns></returns>
        [HttpPost("{id}/data/{pcid}/coordinates")]
        public ActionResult PostCoordinates(string id, string pcid, [FromBody] List<Coordinate> coordinates)
        {
            _dataRepository.AppendToPointCollection(id, pcid, coordinates, _userIdService.GetUserID());
            return Ok();
        }

        /// <summary>
        /// Creates a new CoordinateData item and returns the id
        /// </summary>
        /// <param name="coordinates">List of {lat,lon,ele,time}</param>
        /// <returns>New Coordinatedata id</returns>
        [HttpPost("data/coordinates")]
        public ActionResult<string> PostCoordinatesToNewObject([FromBody] List<Coordinate> coordinates)
        {
            CoordinateData data = new CoordinateData();
            data.Data.Add(new PointCollection() { ID = "1", Coordinates = coordinates });
            var newId = _dataRepository.Create(data, _userIdService.GetUserID());
            return newId;
        }

        /// <summary>
        /// Get Coordinate data only for given CoordinateData.ID, PointCollection.ID
        /// </summary>
        /// <param name="id">CoordinateData.ID</param>
        /// <param name="pcid">PointCollection.ID</param>
        /// <returns>List of {lat,lon,ele,time}</returns>
        [HttpGet("{id}/data/{pcid}/coordinates")]
        public ActionResult<List<Coordinate>> GetCoordinates(string id, string pcid)
        {
            CoordinateData data;
            data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

            if (data != null && data.Data.Any())
            {
                var coords = data.Data.Where(x => x.ID == pcid).FirstOrDefault();
                if (coords != null) return coords.Coordinates.ToList();
            }

            return NotFound();
        }

        /// <summary>
        /// Update CoordinateData
        /// </summary>
        /// <param name="id">CoordinateData.ID</param>
        /// <param name="coordinateData">CoordinateData</param>
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] CoordinateData coordinateData)
        {
            _dataRepository.Update(id, coordinateData, _userIdService.GetUserID());
        }

        /// <summary>
        /// Delete CoordinateData
        /// </summary>
        /// <param name="id">CoordinateData.ID</param>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _dataRepository.Delete(id, _userIdService.GetUserID());
            return Ok();
        }

    }
}