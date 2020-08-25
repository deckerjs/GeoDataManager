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
        private readonly IImageToCoordinateDataTransform _imageToCoordTransform;

        public ImageUploadController(ICoordinateDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IImageToCoordinateDataTransform transform)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
            _imageToCoordTransform = transform;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ImageUpload imageUpload, [FromQuery] string coordinateDataId)
        {
            if (imageUpload != null && imageUpload.ImageData != null)
            {
                var data = _imageToCoordTransform.GetCoordinateData(new MemoryStream(imageUpload.ImageData));
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

        [HttpPost("{coordinateDataId}")]
        public IActionResult PostToExistingDataSet([FromBody] ImageUpload imageUpload, string coordinateDataId)
        {
            if (imageUpload != null && imageUpload.ImageData != null)
            {
                var user = _userIdService.GetUserID();
                var coordinateData = _dataRepository.GetSingle(coordinateDataId, user);
                if (coordinateData != null)
                {
                    var pointCollection = _imageToCoordTransform.GetPointCollection(new MemoryStream(imageUpload.ImageData));
                    if (pointCollection.Coordinates.Any())
                    {
                        coordinateData.Data.Add(pointCollection);
                        _dataRepository.Update(coordinateData.ID, coordinateData, user);
                        return Ok();
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest("Not Created. Missing data");
            }
        }

        //add option to add to exiting set
        //add bulk load option

    }
}
