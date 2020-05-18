using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IUserRolesRepository _rolesRepository;

        public UserAdminController(IUserRepository userRepository, IUserRolesRepository rolesRepository)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
        }

        [HttpGet]        
        public ActionResult<IEnumerable<AppUser>> Get()
        {
            var filters = new List<Expression<Func<AppUser, bool>>>();
            return _userRepository.GetAllUsers(filters).ToList();
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

        [Route("Roles/{userId}")]
        [HttpGet]
        public ActionResult<AppUserRoles> GetRoles(string userId)
        {
            return _rolesRepository.GetUserRoles(userId);
        }

        [Route("Roles/{userId}")]
        [HttpPut]
        public ActionResult UpdateRoles(string userId, AppUserRoles roles)
        {
            _rolesRepository.UpdateUserRoles(userId, roles);
            return Ok();
        }


    }
}





