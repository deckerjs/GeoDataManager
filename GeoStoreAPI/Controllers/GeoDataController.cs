using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class GeoDataController : ControllerBase
    {
        private readonly IGeoDataRepository _dataRepository;
        private readonly IUserIdentificationService _userIdService;

        public GeoDataController(
            IGeoDataRepository dataRepository,
            IUserIdentificationService userIdService)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GeoData>> GetAll([FromQuery]string filterCriteria)
        {
            //todo: filter criteria builder from incoming filterCriteria
            return _dataRepository.GetAll(_userIdService.GetUserID(), (x) => true).ToList();
        }

        [HttpGet]
        [Route("Shared")]
        public ActionResult<IEnumerable<GeoData>> GetShared([FromQuery]string filterCriteria)
        {
            return _dataRepository.GetShared(_userIdService.GetUserID(), (x) => true).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GeoData> Get(string id)
        {
            return _dataRepository.GetSingle(id, _userIdService.GetUserID());
        }

        [HttpPost]
        public string Post([FromBody] GeoData geoData)
        {
            return _dataRepository.Create(geoData, _userIdService.GetUserID());
        }

        [HttpPost("{id}/data/features/geometry/coordinates")]
        public ActionResult<string> PostCoordinates(string id, [FromBody] List<Coordinate> coordinates)
        {
            GeoData data;

            data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

            if (data != null)
            {
                _dataRepository.AppendMultiPointCollection(data.ID, coordinates);
                return data.ID;
            }

            return NotFound();
        }

        [HttpPost("data/features/geometry/coordinates")]
        public ActionResult<string> PostCoordinatesToNewObject([FromBody] List<Coordinate> coordinates)
        {
            GeoData data;

            var newId = _dataRepository.Create(new GeoData(), _userIdService.GetUserID());
            data = _dataRepository.GetSingle(newId, _userIdService.GetUserID());

            _dataRepository.AppendMultiPointCollection(data.ID, coordinates);

            return data.ID;
        }

        [HttpGet("{id}/data/features/geometry/coordinates")]
        public ActionResult<List<Coordinate>> GetCoordinates(string id)
        {
            GeoData data;

            data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

            if (data != null)
            {
                return _dataRepository.GetCoordinatesFromFeatureCollection(data.Data);
            }

            return NotFound();
        }


        [HttpPut("{id}")]
        public void Put(string id, [FromBody] GeoData geoData)
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