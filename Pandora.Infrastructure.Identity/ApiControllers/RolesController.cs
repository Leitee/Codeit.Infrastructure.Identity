/// <summary>
/// 
/// </summary>
namespace Pandora.Infrastructure.Identity.ApiControllers
{
    using Pandora.Infrastructure.Identity.Interfaces;
    using Pandora.Infrastructure.Identity.Model.Dtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Pandora.NetStdLibrary.Base.Application;
    using Pandora.NetStdLibrary.Base.Constants;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    //[Authorize(Roles = Roles.Admin)]
    public class RolesController : ApiBaseController
    {
        protected readonly IRoleService roleService;

        public RolesController(IRoleService roleService, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.roleService = roleService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Assign(string id, RoleDTO role)
        {
            var result = await roleService.AssignToRole(id, role.Name);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> Unassign(string id, RoleDTO role)
        {
            var result = await roleService.UnassignRole(id, role.Name);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors.FirstOrDefault());
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRoles(string id)
        {
            var result = await roleService.GetRoles(id);
            return Ok(result);
        }
    }
}
