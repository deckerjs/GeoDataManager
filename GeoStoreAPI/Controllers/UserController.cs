using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public void Post([FromBody] AppUser user)
        {
            //todo: need more limited user object, no providing an id, or roles
            //limited update, this is unauthorized new user creation
            _userRepository.CreateUser(user);
        }


        [Authorize(Roles = "user")]
        [HttpGet("{userID}")]
        public ActionResult<AppUser> Get(string userID)
        {
            //todo: need more limited user object, don't return pw
            //limited return, and limited to logged in user
            return _userRepository.GetUser(userID);
        }

        [Authorize(Roles = "user")]
        [HttpPut]
        public void Put([FromBody] AppUser user)
        {
            //todo: need more limited user object, no changing an id, or roles
            //limited update, and limited to logged in user
            _userRepository.UpdateUser(user);
        }

    }
}