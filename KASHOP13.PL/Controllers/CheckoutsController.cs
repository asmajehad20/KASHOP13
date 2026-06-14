using KASHOP13.BLL.Service;
using KASHOP13.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KASHOP13.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        
        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody]CheckoutRequest request)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _checkoutService.ProcessCheckout(UserId, request);

            if (!response.Success) return BadRequest();

            return Ok(new
            {
                response,

            });
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success()
        {
            return Ok(new
            {
                Message = "Success"
            });
        }

        [HttpGet("cancel")]
        public async Task<IActionResult> Cancel()
        {
            return Ok(new
            {
                Message = "Payment failed"
            });
        }
    }
}
