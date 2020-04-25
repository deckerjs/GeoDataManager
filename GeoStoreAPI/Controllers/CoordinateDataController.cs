using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoordinateDataModels;
using GeoDataModels.Models;
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

        [HttpGet]
        public ActionResult<IEnumerable<CoordinateData>> GetAll()
        {
            Func<CoordinateData, bool> filter = _filterBuilder.GetFilter<CoordinateData>();

            var result = _dataRepository.GetAll(_userIdService.GetUserID(), filter);
            if (result != null) return result.ToList();
            return null;
        }

        [HttpGet]
        [Route("Shared")]
        public ActionResult<IEnumerable<CoordinateData>> GetShared()
        {
            Func<CoordinateData, bool> filter = _filterBuilder.GetFilter<CoordinateData>();
            return _dataRepository.GetShared(_userIdService.GetUserID(), filter).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<CoordinateData> Get(string id)
        {
            return _dataRepository.GetSingle(id, _userIdService.GetUserID());
        }

        [HttpPost]
        public string Post([FromBody] CoordinateData geoData)
        {
            return _dataRepository.Create(geoData, _userIdService.GetUserID());
        }

        //[HttpPost("{id}/data/features/geometry/coordinates")]
        //public ActionResult<string> PostCoordinates(string id, [FromBody] List<Coordinate> coordinates)
        //{
        //    CoordinateData data;

        //    data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

        //    if (data != null)
        //    {
        //        _dataRepository.AppendMultiPointCollection(data.ID, coordinates);
        //        return data.ID;
        //    }

        //    return NotFound();
        //}

        //[HttpPost("data/features/geometry/coordinates")]
        //public ActionResult<string> PostCoordinatesToNewObject([FromBody] List<Coordinate> coordinates)
        //{
        //    CoordinateData data;

        //    var newId = _dataRepository.Create(new CoordinateData(), _userIdService.GetUserID());
        //    data = _dataRepository.GetSingle(newId, _userIdService.GetUserID());

        //    _dataRepository.AppendMultiPointCollection(data.ID, coordinates);

        //    return data.ID;
        //}

        //[HttpGet("{id}/data/features/geometry/coordinates")]
        //public ActionResult<List<Coordinate>> GetCoordinates(string id)
        //{
        //    CoordinateData data;

        //    data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

        //    if (data != null)
        //    {
        //        return _dataRepository.GetCoordinatesFromFeatureCollection(data.Data);
        //    }

        //    return NotFound();
        //}


        [HttpPut("{id}")]
        public void Put(string id, [FromBody] CoordinateData geoData)
        {
            _dataRepository.Update(id, geoData, _userIdService.GetUserID());
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _dataRepository.Delete(id, _userIdService.GetUserID());
        }

    }
}