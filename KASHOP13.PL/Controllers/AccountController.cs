using KASHOP13.BLL.Service;
using KASHOP13.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP13.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var isConfimed = await _authenticationService.ConfirmEmailAsync(token, userId);
            if(isConfimed) return Ok(new { message = "Success" });
            return BadRequest();
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordResetAsync(request);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordRequest(ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authenticationService.RefreshTokenAsync();
            if (!result.Success) return Unauthorized(result);
            return Ok(result);
        }
    }
}
