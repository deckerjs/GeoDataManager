using DataTransformUtilities.Transformers;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public IActionResult Post([FromBody] ImageUpload imageUpload)
        {
            if (imageUpload != null && imageUpload.ImageData != null)
            {
                var data = _gpxTransform.GetCoordinateData(new MemoryStream(imageUpload.ImageData));
                if (data.Data.FirstOrDefault().Coordinates.Any())
                {
                    _dataRepository.Create(data, _userIdService.GetUserID());
                    //todo: return Created(url) for new object
                    return Ok();
                }
                return NoContent();
            }
            else
            {
                return BadRequest("Not Created. Missing data");
            }
        }

    }
}
