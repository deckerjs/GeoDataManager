using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoStoreAPI.Services
{
    public class UserIdentificationService : IUserIdentificationService
    {
        private readonly IHttpContextAccessor _context;
        public UserIdentificationService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUserID()
        {
            //new System.Collections.Generic.IDictionaryDebugView<string, string>((new System.Linq.SystemCore_EnumerableDebugView<System.Security.Claims.Claim>(_context.HttpContext.User.Claims).Items[6]).Properties).Items[0]
            //var userIdClaim = _context.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault();
            var userId = _context.HttpContext.User.Claims.Where(x => x.Properties.Any(y=>y.Value == "sub")).FirstOrDefault();

            if (userId != null)
            {
                return userId.Value;
            }
            return string.Empty;
        }
    }

}

