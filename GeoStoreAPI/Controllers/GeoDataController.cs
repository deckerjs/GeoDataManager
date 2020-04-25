using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoordinateDataModels;
using DataTransformUtilities.Transformers;
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
        private readonly ICoordinateDataRepository _dataRepository;
        private readonly IUserIdentificationService _userIdService;
        private readonly IQueryStringFilterBuilderService _filterBuilder;

        public GeoDataController(
            ICoordinateDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IQueryStringFilterBuilderService filterBuilder)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
            _filterBuilder = filterBuilder;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GeoJsonData>> GetAll()
        {
            Func<CoordinateData, bool> filter = _filterBuilder.GetFilter<CoordinateData>();

            var result = _dataRepository.GetAll(_userIdService.GetUserID(), filter).ToList();            
             
            if (result != null) return GetGeoJsonFromCoordinateData(result);
            return null;
        }


        [HttpGet]
        [Route("Shared")]
        public ActionResult<IEnumerable<GeoJsonData>> GetShared()
        {
            Func<CoordinateData, bool> filter = _filterBuilder.GetFilter<CoordinateData>();
            var result = _dataRepository.GetShared(_userIdService.GetUserID(), filter).ToList();

            if (result != null) return GetGeoJsonFromCoordinateData(result);
            return null;
        }

        [HttpGet("{id}")]
        public ActionResult<GeoJsonData> Get(string id)
        {
            var result = _dataRepository.GetSingle(id, _userIdService.GetUserID());

            if (result != null) return GetGeoJsonFromCoordinateData(result);
            return null;
        }

        //[HttpPost]
        //public string Post([FromBody] GeoJsonData geoData)
        //{
        //    return _dataRepository.Create(geoData, _userIdService.GetUserID());
        //}

        //[HttpPost("{id}/data/features/geometry/coordinates")]
        //public ActionResult<string> PostCoordinates(string id, [FromBody] List<Coordinate> coordinates)
        //{
        //    //GeoJsonData data;

        //    //data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

        //    //if (data != null)
        //    //{
        //    //    _dataRepository.AppendMultiPointCollection(data.ID, coordinates);
        //    //    return data.ID;
        //    //}

        //    //return NotFound();
            
        //}

        //[HttpPost("data/features/geometry/coordinates")]
        //public ActionResult<string> PostCoordinatesToNewObject([FromBody] List<Coordinate> coordinates)
        //{
        //    //GeoJsonData data;

        //    //var newId = _dataRepository.Create(new GeoJsonData(), _userIdService.GetUserID());
        //    //data = _dataRepository.GetSingle(newId, _userIdService.GetUserID());

        //    //_dataRepository.AppendMultiPointCollection(data.ID, coordinates);

        //    //return data.ID;
            
        //}

        [HttpGet("{id}/data/features/geometry/coordinates")]
        public ActionResult<List<CoordinateDataModels.Coordinate>> GetCoordinates(string id)
        {
            //GeoJsonData data;

            //data = _dataRepository.GetSingle(id, _userIdService.GetUserID());

            //if (data != null)
            //{
            //    return _dataRepository.GetCoordinatesFromFeatureCollection(data.Data);
            //}

            return NotFound();
        }


        //[HttpPut("{id}")]
        //public void Put(string id, [FromBody] GeoJsonData geoData)
        //{
        //    _dataRepository.Update(id, geoData, _userIdService.GetUserID());
        //}

        //[HttpDelete("{id}")]
        //public void Delete(string id)
        //{
        //    _dataRepository.Delete(id, _userIdService.GetUserID());
        //}

        private GeoJsonData GetGeoJsonFromCoordinateData(CoordinateData coordinateData)
        {
            return CoordinateDataToGeoJsonTransformer.GetGeoJsonFromCoordinateData(coordinateData);
        }

        private List<GeoJsonData> GetGeoJsonFromCoordinateData(List<CoordinateData> coordinateData)
        {
            var geoJsonData = new List<GeoJsonData>();
            coordinateData.ForEach(x => geoJsonData.Add(CoordinateDataToGeoJsonTransformer.GetGeoJsonFromCoordinateData(x)));
            return geoJsonData;
        }
    }
}