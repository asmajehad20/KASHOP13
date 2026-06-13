using KASHOP13.BLL.Service;
using KASHOP13.DAL.DTO.Request;
using KASHOP13.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP13.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICartService _cartService;
        public CartsController(IStringLocalizer<SharedResources> localizer, ICartService cartService)
        {
            _localizer = localizer;
            _cartService = cartService;
        }

        [HttpPost("")]
        
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCart(request, UserId);

            if (!result) return BadRequest();

            return Ok(new
            {
                Message = _localizer["Success"].Value,
               
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCart()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _cartService.GetCart(UserId);

            return Ok(new
            {
                data = items
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveItem([FromRoute]int id)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removed = await _cartService.RemoveItem(id, UserId);

            if (!removed) return BadRequest();

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute]int id, [FromBody]UpdateCartRequest request)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _cartService.UpdateQuantity(id, request.Count, UserId);

            if (!updated) return BadRequest();
            return Ok();
        }
    }
}
