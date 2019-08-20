using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Models;
using System;

namespace GeoStoreAPI.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ILogger<CustomProfileService> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IUserRolesRepository _userRolesRepository;

        public CustomProfileService(
            IUserRepository userRepository,
            IUserRolesRepository userRolesRepository,
            ILogger<CustomProfileService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userRolesRepository = userRolesRepository;
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
                new Claim("username", user.UserName),
                new Claim("email", user.Email)
            };
            claims.AddRange(GetUserRoleClaims(user.ID));

            context.IssuedClaims = claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(string userID)
        {
            var claims = new List<Claim>();
            var userRoles = _userRolesRepository.GetUserRoles(userID);

            foreach (var roleID in userRoles.RoleIDs)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleID));
            }

            return claims;
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