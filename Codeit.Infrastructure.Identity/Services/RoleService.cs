using Codeit.Infrastructure.Identity.Interfaces;
using Codeit.Infrastructure.Identity.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Codeit.NetStdLibrary.Base.Identity;
using SendGrid.Helpers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codeit.Infrastructure.Identity.Services
{
    public class RoleService : IRoleService
    {
        protected readonly UserManager<IdentityAppUser> userManager;
        protected readonly RoleManager<ApplicationRole> roleManager;
        private readonly IdentityErrorDescriber _errorHelper;

        public RoleService(UserManager<IdentityAppUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _errorHelper = new IdentityErrorDescriber();
        }

        public async Task<IdentityResult> AssignToRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (await roleManager.RoleExistsAsync(roleName) && user != null)
            {
                return await userManager.AddToRoleAsync(user, roleName);
            }

            return IdentityResult.Failed(_errorHelper.InvalidRoleName(roleName));
        }

        public async Task<IdentityResult> UnassignRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (await roleManager.RoleExistsAsync(roleName) && user != null)
            {
                return await userManager.RemoveFromRoleAsync(user, roleName);
            }

            return IdentityResult.Failed(_errorHelper.InvalidRoleName(roleName));
        }

        public async Task<IList<string>> GetRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return await userManager.GetRolesAsync(user);
        }
    }
}
