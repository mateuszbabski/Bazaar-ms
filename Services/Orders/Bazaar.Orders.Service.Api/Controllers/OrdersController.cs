using Bazaar.Orders.Service.Commands.Handlers.CancelOrder;
using Bazaar.Orders.Service.Commands.Handlers.PayForOrder;
using Bazaar.Orders.Service.Commands.Handlers.PlaceOrder;
using Bazaar.Orders.Service.Commands.Responses;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Orders.Service.Queries.Handlers.GetOrderById;
using Bazaar.Orders.Service.Queries.Handlers.GetOrdersByCustomerId;
using Bazaar.Orders.Service.Queries.Handlers.GetOrdersByShopId;
using Bazaar.Orders.Service.Queries.Responses;
using Bazaar.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Orders.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly PlaceOrderCommandHandler _placeOrderHandler;
        private readonly PayForOrderCommandHandler _payForOrderHandler;
        private readonly CancelOrderCommandHandler _cancelOrderHandler;
        private readonly GetOrdersByCustomerIdQueryHandler _getOrdersByCustomerIdHandler;
        private readonly GetOrderByIdQueryHandler _getOrderByIdHandler;
        private readonly GetOrdersByShopIdQueryHandler _getOrdersByShopIdHandler;

        public OrdersController(
            PlaceOrderCommandHandler placeOrderHandler,
            PayForOrderCommandHandler payForOrderHandler,
            CancelOrderCommandHandler cancelOrderHandler,
            GetOrdersByCustomerIdQueryHandler getOrdersByCustomerIdHandler,
            GetOrderByIdQueryHandler getOrderByIdHandler,
            GetOrdersByShopIdQueryHandler getOrdersByShopIdHandler)
        {
            _placeOrderHandler = placeOrderHandler;
            _payForOrderHandler = payForOrderHandler;
            _cancelOrderHandler = cancelOrderHandler;
            _getOrdersByCustomerIdHandler = getOrdersByCustomerIdHandler;
            _getOrderByIdHandler = getOrderByIdHandler;
            _getOrdersByShopIdHandler = getOrdersByShopIdHandler;
        }

        [HttpPost]
        public async Task<ActionResult<PlaceOrderResult>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var result = await _placeOrderHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPost("{orderId}/pay")]
        public async Task<ActionResult<PayForOrderResult>> PayForOrder(Guid orderId)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var command = new PayForOrderCommand
            {
                OrderId = orderId,
                CustomerId = Guid.Parse(userId)
            };

            var result = await _payForOrderHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.OrderNotFound => NotFound(result),
                ErrorCode.Forbidden => StatusCode(403, result),
                ErrorCode.InvalidOrderStatus => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPatch("{orderId}/cancel")]
        public async Task<ActionResult<CancelOrderResult>> CancelOrder(Guid orderId)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var command = new CancelOrderCommand
            {
                OrderId = orderId,
                CustomerId = Guid.Parse(userId)
            };

            var result = await _cancelOrderHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.OrderNotFound => NotFound(result),
                ErrorCode.Forbidden => StatusCode(403, result),
                ErrorCode.InvalidOrderStatus => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet]
        public async Task<ActionResult<GetOrdersResult>> GetOrders([FromQuery] OrderStatus? status)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var query = new GetOrdersByCustomerIdQuery
            {
                CustomerId = Guid.Parse(userId),
                Status = status
            };

            var result = await _getOrdersByCustomerIdHandler.Handle(query);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.OrdersNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<GetOrderByIdResult>> GetOrderById(Guid orderId)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var query = new GetOrderByIdQuery
            {
                OrderId = orderId,
                CustomerId = Guid.Parse(userId)
            };

            var result = await _getOrderByIdHandler.Handle(query);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.OrderNotFound => NotFound(result),
                ErrorCode.Forbidden => StatusCode(403, result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet("shop")]
        public async Task<ActionResult<GetOrdersResult>> GetShopOrders([FromQuery] OrderStatus? status)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Shop), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var query = new GetOrdersByShopIdQuery
            {
                ShopId = Guid.Parse(userId),
                Status = status
            };

            var result = await _getOrdersByShopIdHandler.Handle(query);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.OrdersNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }
    }
}
