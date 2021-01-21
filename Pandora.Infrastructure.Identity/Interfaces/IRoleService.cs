using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Infrastructure.Identity.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> AssignToRole(string userId, string roleName);
        Task<IdentityResult> UnassignRole(string userId, string roleName);
        Task<IList<string>> GetRoles(string userId);
    }
}
