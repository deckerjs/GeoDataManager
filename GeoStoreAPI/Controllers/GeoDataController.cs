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
        public string PostCoordinates(string id, [FromBody] List<Position> coordinates)
        {
            GeoData data;

            if (string.IsNullOrEmpty(id))
            {
                var newId = _dataRepository.Create(new GeoData(), _userIdService.GetUserID());
                data = _dataRepository.GetSingle(newId, _userIdService.GetUserID());
            }
            else
            {
                data = _dataRepository.GetSingle(id, _userIdService.GetUserID());
            }

            var featureCollection = _dataRepository.GetCoordinatesFeatureCollection(coordinates);
            _dataRepository.AppendFeatureCollection(data.ID, featureCollection);

            return data.ID;
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