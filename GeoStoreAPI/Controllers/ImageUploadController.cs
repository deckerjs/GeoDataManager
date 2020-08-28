using DataTransformUtilities.Transformers;
using DnsClient.Internal;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        private readonly ILogger<ImageUploadController> _logger;
        private static readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1);

        public ImageUploadController(ICoordinateDataRepository dataRepository,
            IUserIdentificationService userIdService,
            IImageToCoordinateDataTransform transform,
            ILogger<ImageUploadController> logger)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
            _imageToCoordTransform = transform;
            _logger = logger;
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


        // todo: deal with this situation better, without locking.
        [HttpPost("{coordinateDataId}")]
        public IActionResult PostToExistingDataSet([FromBody] ImageUpload imageUpload, string coordinateDataId)
        {
            if (imageUpload != null && imageUpload.ImageData != null)
            {
                var user = _userIdService.GetUserID();

                _updateLock.Wait();

                try
                {
                    var coordinateData = _dataRepository.GetSingle(coordinateDataId, user);
                    if (coordinateData != null)
                    {
                        var pointCollection = _imageToCoordTransform.GetPointCollection(new MemoryStream(imageUpload.ImageData));
                        if (pointCollection.Coordinates.Any())
                        {
                            coordinateData.Data.Add(pointCollection);
                            _dataRepository.Update(coordinateData.ID, coordinateData, user);
                            _updateLock.Release();
                            return Ok();
                        }
                        else
                        {
                            _updateLock.Release();
                            return NoContent();
                        }
                    }
                    else
                    {
                        _updateLock.Release();
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _updateLock.Release();
                    return StatusCode(500);
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
