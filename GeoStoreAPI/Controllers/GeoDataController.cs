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

        public GeoDataController(IGeoDataRepository dataRepository, IUserIdentificationService userIdService)
        {
            _dataRepository = dataRepository;
            _userIdService = userIdService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GeoData>> Get([FromQuery]string filterCriteria)
        {   
            //todo: filter criteria builder
            return _dataRepository.GetAll(_userIdService.GetUserID(), (x)=>true).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<GeoData> Get(Guid id)
        {
            return _dataRepository.GetSingle(id, _userIdService.GetUserID());
        }

        [HttpPost]
        public void Post([FromBody] GeoData geoData)
        {
            _dataRepository.Create(geoData, _userIdService.GetUserID());
        }

        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] GeoData geoData)
        {
            _dataRepository.Update(id, geoData, _userIdService.GetUserID());
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _dataRepository.Delete(id, _userIdService.GetUserID());
        }
        
        // private IEnumerable<GeoData> GetMockData()
        // {
        //     var testData = new List<GeoData>();
        //     var data1 = new GeoData()
        //     {
        //         UserID = "testuser1234",
        //         ID = Guid.NewGuid().ToString(),
        //         DateCreated = DateTime.Now,
        //         Description = "test description",
        //         Tags = { "some tag", "some other tag", "tag 3" },
        //         DateModified = DateTime.Now,
        //         Data = GetSampleFeatureCollection()
        //     };
        //     testData.Add(data1);
        //     return testData;
        // }
        // private Models.FeatureCollection GetSampleFeatureCollection()
        // {
        //     var coords = new List<Position>(){
        //         new Position(-88.095398000000003, 42.918624000000001, 252.93600000000001),
        //         new Position(-88.095395999999994, 42.918604000000002, 245.953),
        //         new Position(-88.095110000000005, 42.918570000000003, 241.78899999999999)
        //     };            

        //     var geom1 = new MultiPoint(coords);

        //     var props = new Dictionary<string, object>();
        //     props["Name"] = "prop 1 name";

        //     var feature1 = new Feature(geom1, props);

        //     var features = new List<Feature>();
        //     features.Add(feature1);

        //     var data = new Models.FeatureCollection(features);

        //     return data;
        // }
    }
}