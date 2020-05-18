using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var filter = _filterBuilder.GetFilter<CoordinateData>();

            var result = _dataRepository.GetAll(_userIdService.GetUserID(), filter).ToList();

            if (result != null) return GetGeoJsonFromCoordinateData(result);
            return null;
        }


        [HttpGet]
        [Route("Shared")]
        public ActionResult<IEnumerable<GeoJsonData>> GetShared()
        {
            var filter = _filterBuilder.GetFilter<CoordinateData>();
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