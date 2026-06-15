using KASHOP13.BLL.Service;
using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.Models;
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
    public class OrdersController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IOrderService _orderService;
        public OrdersController(IStringLocalizer<SharedResources> localizer, IOrderService orderService)
        {
            _localizer = localizer;
            _orderService = orderService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUserOrders()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrder(UserId);

            return Ok(new {data = orders});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetUserOrder(UserId, id);

            return Ok(new { data = order });
        }

        [HttpGet("admin")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderByStatus([FromQuery]OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetAllOrders(status);

            return Ok(new { data = orders });
        }

        [HttpPatch("admin/{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody]ChangeOrderStatusRequest status)
        {
            var result = await _orderService.ChangeOrderStatus(id, status);
            if(!result) return BadRequest(result);

            return Ok(result);
        }
    }
}
