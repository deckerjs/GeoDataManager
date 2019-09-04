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

        public UserController(IUserRepository userRepository, IUserIdentificationService userIdService)
        {
            _userRepository = userRepository;
            _userIdService = userIdService;
        }

        [HttpPost]
        public void Post([FromBody] AppUserBase newUser)
        {
            string userID = _userIdService.GetUserID();
            var user = new AppUser(newUser);
            
            _userRepository.CreateUser(user);
        }


        [Authorize(Roles = "user")]
        [HttpGet("{userID}")]
        public ActionResult<AppUser> Get()
        {
            string userID =  _userIdService.GetUserID();
            var user = _userRepository.GetUser(userID);
            user.Password = "********";
            return user;
        }

        [Authorize(Roles = "user")]
        [HttpPut]
        public void Put([FromBody] AppUserBase userUpdate)
        {
            string userID =  _userIdService.GetUserID();
            var user = _userRepository.GetUser(userID);
            user.UpdateWith(userUpdate);
            _userRepository.UpdateUser(user);
        }

    }
}