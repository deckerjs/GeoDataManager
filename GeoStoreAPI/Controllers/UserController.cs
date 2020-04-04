using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserIdentificationService _userIdService;
        private readonly IUserDataPermissionRepository _dataPermissionRepository;

        public UserController(IUserRepository userRepository, 
            IUserIdentificationService userIdService,
            IUserDataPermissionRepository dataPermissionRepository)
        {
            _userRepository = userRepository;
            _userIdService = userIdService;
            _dataPermissionRepository = dataPermissionRepository;
        }

        [HttpPost]
        public void Post([FromBody] AppUserBase newUser)
        {
            string userID = _userIdService.GetUserID();
            var user = new AppUser(newUser);

            _userRepository.CreateUser(user);
        }


        [Authorize(Roles = "user")]
        [HttpGet]
        public ActionResult<AppUser> Get()
        {
            string userID = _userIdService.GetUserID();
            var user = _userRepository.GetUser(userID);
            user.Password = "********";
            return user;
        }

        [Authorize(Roles = "user")]
        [HttpPut]
        public void Put([FromBody] AppUserBase userUpdate)
        {
            string userID = _userIdService.GetUserID();
            var user = _userRepository.GetUser(userID);
            user.UpdateWith(userUpdate);
            _userRepository.UpdateUser(user);
        }

        [Authorize(Roles = "user")]
        [Route("DataPermissions")]
        [HttpPost]
        public void Post([FromBody] UserDataPermission dataPermission)
        {
            string userID = _userIdService.GetUserID();
            _dataPermissionRepository.Create(dataPermission,userID);
        }

        [Authorize(Roles = "user")]
        [Route("DataPermissions")]
        [HttpGet]
        public ActionResult<List<UserDataPermission>> GetDataPermissions()
        {
            Func<UserDataPermission, bool> filter = x => true;
            string userID = _userIdService.GetUserID();
            return _dataPermissionRepository.GetAllForOwnerUser(userID,filter).ToList();
        }

        [Authorize(Roles = "user")]
        [Route("DataPermissions/{dataPermissionId}")]
        [HttpGet]
        public ActionResult<UserDataPermission> GetDataPermission(string dataPermissionId)
        {            
            string userID = _userIdService.GetUserID();
            return _dataPermissionRepository.Get(dataPermissionId, userID);
        }

        [Authorize(Roles = "user")]
        [Route("DataPermissions")]
        [HttpPut]
        public IActionResult UpdateDataPermission([FromBody] UserDataPermission dataPermission)
        {
            string userID = _userIdService.GetUserID();
            _dataPermissionRepository.Update(dataPermission, userID);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [Route("DataPermissions/{dataPermissionId}")]
        [HttpDelete]
        public IActionResult DeleteDataPermission(string dataPermissionId)
        {
            string userID = _userIdService.GetUserID();
            _dataPermissionRepository.Delete(dataPermissionId, userID);
            return Ok();
        }

        [Authorize(Roles = "user")]
        [Route("DataPermissions/Granted")]
        [HttpGet]
        public ActionResult<List<UserDataPermission>> GetDataPermissionsGranted()
        {
            Func<UserDataPermission, bool> filter = x => true;
            string userID = _userIdService.GetUserID();
            return _dataPermissionRepository.GetAllGrantedToUser(userID,filter).ToList();
        }

    }
}