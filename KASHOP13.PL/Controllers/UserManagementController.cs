using KASHOP13.BLL.Service;
using KASHOP13.DAL.DTO.Request;
using KASHOP13.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP13.PL.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserManagementService _userManagementtService;
        public UserManagementController(IStringLocalizer<SharedResources> localizer, IUserManagementService userManagementtService)
        {
            _localizer = localizer;
            _userManagementtService = userManagementtService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementtService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var user = await _userManagementtService.GetUser(id);
            return Ok(user);
        }

        [HttpPatch("users/{id}/role")]
        public async Task<IActionResult> ChangeRole(string id,[FromBody] ChangeRoleRequest request)
        {
            var result = await _userManagementtService.ChangeRole(id, request.newRole);

            if(!result) return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("users/{id}/toggleblock")]
        public async Task<IActionResult> ToggleBlock(string id)
        {
            var result = await _userManagementtService.ToggleBlockUser(id);

            if (!result) return BadRequest(result);

            return Ok(result);
        }
    }
}
