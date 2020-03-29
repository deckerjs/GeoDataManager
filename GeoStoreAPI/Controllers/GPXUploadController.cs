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

namespace GeoStoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GPXUploadController : ControllerBase
    {
        private readonly IGeoDataRepository _dataRepository;
        private readonly IUserIdentificationService _userIdService;
        private readonly IGPXTransform _gpxTransform;

        public GPXUploadController(IGeoDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IGPXTransform gpxTransform)
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
                var geoData = _gpxTransform.GetGeoDataFromGpx(gpx);
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