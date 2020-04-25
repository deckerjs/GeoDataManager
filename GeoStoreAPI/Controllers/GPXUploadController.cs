using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataTransformUtilities.Models;
using DataTransformUtilities.Transformers;
using CoordinateDataModels;

namespace GeoStoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GPXUploadController : ControllerBase
    {
        private readonly ICoordinateDataRepository _dataRepository;
        private readonly IUserIdentificationService _userIdService;
        private readonly IGPXTransform<CoordinateData> _gpxTransform;
        
        public GPXUploadController(ICoordinateDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IGPXTransform<CoordinateData> gpxTransform)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
            _gpxTransform = gpxTransform;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Gpx gpx)
        {
            if (gpx != null )
            {
                var geoData = _gpxTransform.GetDataFromGpx(gpx);
                _dataRepository.Create(geoData, _userIdService.GetUserID());
                //todo: return Created(url) for new object
                return Ok();
            }
            else
            {
                return BadRequest("Not Created. Missing Gpx data");
            }
        }

    }
}