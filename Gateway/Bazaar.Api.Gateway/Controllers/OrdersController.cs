using Bazaar.Api.Gateway.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersServiceClient _serviceClient;

        public OrdersController(OrdersServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetOrders([FromQuery] string? status)
        {
            var response = await _serviceClient.GetOrders(status);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<ActionResult> GetOrderById(Guid orderId)
        {
            var response = await _serviceClient.GetOrderById(orderId);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("{orderId}/pay")]
        [Authorize]
        public async Task<ActionResult> PayForOrder(Guid orderId)
        {
            var response = await _serviceClient.PayForOrder(orderId);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("{orderId}/cancel")]
        [Authorize]
        public async Task<ActionResult> CancelOrder(Guid orderId)
        {
            var response = await _serviceClient.CancelOrder(orderId);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpGet("shop")]
        [Authorize]
        public async Task<ActionResult> GetShopOrders([FromQuery] string? status)
        {
            var response = await _serviceClient.GetShopOrders(status);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
