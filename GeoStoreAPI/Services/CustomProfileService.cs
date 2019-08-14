using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Models;

namespace GeoStoreAPI.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ILogger<CustomProfileService> _logger;

        private readonly IUserRepository _userRepository;

        public CustomProfileService(IUserRepository userRepository, ILogger<CustomProfileService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            var sub = context.Subject.GetSubjectId();

            _logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            var user = await GetUserAsync(context.Subject.GetSubjectId());
            var claims = new List<Claim>
            {
                //todo: load roles for userid from new roles repo
                new Claim("role", "user"),
                new Claim("username", user.UserName),
                new Claim("email", user.Email)
            };

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {            
            var user = await GetUserAsync(context.Subject.GetSubjectId());
            context.IsActive = user != null;
        }

        private async Task<AppUser> GetUserAsync(string subjectID)
        {
            return await Task.FromResult(_userRepository.FindBySubjectId(subjectID));
        }

    }
}