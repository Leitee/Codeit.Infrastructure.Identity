/// <summary>
/// 
/// </summary>
namespace Codeit.Infrastructure.Identity.Services
{
    using Codeit.Infrastructure.Identity.Model.Entities;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Codeit.Enterprise.Base.DataAccess;
    using IdentityModel;

    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityAppUser> _claimsFactory;
        private readonly UserManager<IdentityAppUser> _userManager;

        public ProfileService(UserManager<IdentityAppUser> userManager, IUserClaimsPrincipalFactory<IdentityAppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            claims.Add(new Claim("join_date", user.JoinDate.ToShortDateString() ?? string.Empty));
            claims.AddRange((await _userManager.GetRolesAsync(user)).Select(x => new Claim(JwtClaimTypes.Role, x)));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}