using Codeit.Infrastructure.Identity.Interfaces;
using Codeit.Infrastructure.Identity.Model.Dtos;
using Codeit.NetStdLibrary.Base.Abstractions.Identity;
using Codeit.NetStdLibrary.Base.Application;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Codeit.Infrastructure.Identity.ApiControllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    [Authorize(LocalApi.PolicyName)]
    public class UsersController : ApiBaseController
    {
        protected readonly IUserService userService;
        protected readonly IAuthService authService;

        public UsersController(IUserService userService, IAuthService authService, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.userService = userService;
            this.authService = authService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            var user = await userService.GetById(id);
            return Ok(user);
        }

        [HttpGet]
        [Route("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrent()
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Subject);
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userService.GetById(userId);
                return Ok(user);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("current")]
        [Authorize]
        public async Task<IActionResult> EditCurrent(UserDTO userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            await userService.Update(userDto);

            var newToken = await authService.GenerateToken(userId);

            return Ok(newToken);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(UserDTO userDto)
        {
            if (!string.IsNullOrEmpty(userDto.Id))
            {
                return BadRequest();
            }

            var result = await userService.Update(userDto);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Edit(string id, UserDTO userDto)
        {
            if (id != userDto.Id)
                return BadRequest();

            var result = await userService.Update(userDto);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.Delete(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("{userId}/photo")]
        [AllowAnonymous]
        public async Task<IActionResult> UserPhoto(string userId)
        {
            //var user = User; // JwtManager.GetPrincipal(token);
            //if (user == null || !user.Identity.IsAuthenticated)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //}

            var photoContent = await userService.GetUserPhoto(userId);

            if (photoContent == null)
            {
                return NoContent();
            }

            return File(photoContent, "image/jpeg");
        }
    }
}
