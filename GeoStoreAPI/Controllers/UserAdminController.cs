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
    [Authorize(Roles="admin")]
    public class UserAdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserAdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]        
        public ActionResult<IEnumerable<AppUser>> Get()
        {
            Func<AppUser, bool> filter = x=> true;
            return _userRepository.GetAllUsers(filter).ToList();
        }

        
        [HttpGet("{userID}")]
        public ActionResult<AppUser> Get(string userID)
        {
            return _userRepository.GetUser(userID) ;
        }

        
        [HttpPost]
        public void Post([FromBody] AppUser user)
        {
            _userRepository.CreateUser(user);
        }

        
        [HttpPut]
        public void Put([FromBody] AppUser user)
        {
            _userRepository.UpdateUser(user);
        }

        
        [HttpDelete("{userID}")]
        public void Delete(string userID)
        {
            _userRepository.DeleteUser(userID);
        }
    }
}





