using DataTransformUtilities.Transformers;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Controllers
{
    public class ImageUploadController : ControllerBase
    {
        private readonly ICoordinateDataRepository _dataRepository;
        private readonly IUserIdentificationService _userIdService;
        private readonly IImageToCoordinateDataTransform _gpxTransform;

        public ImageUploadController(ICoordinateDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IImageToCoordinateDataTransform gpxTransform)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
            _gpxTransform = gpxTransform;
        }

        [HttpPost]
        public IActionResult Post([FromBody] byte[] imageByteArray)
        {
            if (imageByteArray != null)
            {
                var data = _gpxTransform.GetCoordinateData(new MemoryStream(imageByteArray));
                _dataRepository.Create(data, _userIdService.GetUserID());
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
