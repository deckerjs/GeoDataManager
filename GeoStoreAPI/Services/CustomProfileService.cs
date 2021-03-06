﻿using System.Security.Claims;
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
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("username", user.UserName),
                    new Claim("email", user.Email)
                };
                claims.AddRange(GetUserRoleClaims(user.ID));
                context.IssuedClaims = claims;
            }
        }

        private IEnumerable<Claim> GetUserRoleClaims(string userID)
        {
            var claims = new List<Claim>();
            var userRoles = _userRolesRepository.GetUserRoles(userID);
            if (userRoles != null)
            {
                foreach (var roleID in userRoles.RoleIDs)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleID));
                }
            }
            else
            {
                _logger.LogWarning($"UserID:{userID} has no roles");
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
            var user = await Task.FromResult(_userRepository.GetUser(subjectID));
            if (!user.Disabled)
            {
                return user;
            }
            else
            {
                _logger.LogInformation($"Disabled Login attempt {subjectID}");
                return null;
            }
        }

    }
}