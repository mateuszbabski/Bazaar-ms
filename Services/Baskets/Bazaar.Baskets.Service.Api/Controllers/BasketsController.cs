using Bazaar.Baskets.Service.Commands.Handlers.AddProductToBasket;
using Bazaar.Baskets.Service.Commands.Handlers.ChangeBasketCurrency;
using Bazaar.Baskets.Service.Commands.Handlers.ChangeItemQuantity;
using Bazaar.Baskets.Service.Commands.Handlers.CheckoutBasket;
using Bazaar.Baskets.Service.Commands.Handlers.ClearBasket;
using Bazaar.Baskets.Service.Commands.Handlers.RemoveProductFromBasket;
using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Queries.Handlers.GetBasket;
using Bazaar.Baskets.Service.Queries.Responses;
using Bazaar.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Baskets.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly AddProductToBasketCommandHandler _addProductToBasketHandler;
        private readonly RemoveProductFromBasketCommandHandler _removeProductFromBasketHandler;
        private readonly ChangeBasketCurrencyCommandHandler _changeBasketCurrencyHandler;
        private readonly ChangeItemQuantityCommandHandler _changeItemQuantityHandler;
        private readonly CheckoutBasketCommandHandler _checkoutBasketHandler;
        private readonly ClearBasketCommandHandler _clearBasketHandler;
        private readonly GetBasketQueryHandler _getBasketQueryHandler;

        public BasketsController(
            AddProductToBasketCommandHandler addProductToBasketHandler,
            RemoveProductFromBasketCommandHandler removeProductFromBasketHandler,
            ChangeBasketCurrencyCommandHandler changeBasketCurrencyHandler,
            ChangeItemQuantityCommandHandler changeItemQuantityHandler,
            CheckoutBasketCommandHandler checkoutBasketHandler,
            ClearBasketCommandHandler clearBasketHandler,
            GetBasketQueryHandler getBasketQueryHandler)
        {
            _addProductToBasketHandler = addProductToBasketHandler;
            _removeProductFromBasketHandler = removeProductFromBasketHandler;
            _changeBasketCurrencyHandler = changeBasketCurrencyHandler;
            _changeItemQuantityHandler = changeItemQuantityHandler;
            _checkoutBasketHandler = checkoutBasketHandler;
            _clearBasketHandler = clearBasketHandler;
            _getBasketQueryHandler = getBasketQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<GetBasketResult>> GetBasket()
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var result = await _getBasketQueryHandler.Handle(new GetBasketQuery { CustomerId = Guid.Parse(userId) });
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.BasketNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPost("items")]
        public async Task<ActionResult<AddProductToBasketResult>> AddProductToBasket([FromBody] AddProductToBasketCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            command.CustomerId = Guid.Parse(userId);
            var result = await _addProductToBasketHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.ProductNotFound => NotFound(result),
                ErrorCode.CurrencyConversionFailed => BadRequest(result),
                ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpDelete("items/{basketItemId}")]
        public async Task<ActionResult<RemoveProductFromBasketResult>> RemoveProductFromBasket(Guid basketItemId)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var command = new RemoveProductFromBasketCommand
            {
                CustomerId = Guid.Parse(userId),
                BasketItemId = basketItemId
            };

            var result = await _removeProductFromBasketHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.BasketNotFound => NotFound(result),
                ErrorCode.BasketItemNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPatch("currency")]
        public async Task<ActionResult<ChangeBasketCurrencyResult>> ChangeBasketCurrency([FromBody] ChangeBasketCurrencyCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            command.CustomerId = Guid.Parse(userId);
            var result = await _changeBasketCurrencyHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.BasketNotFound => NotFound(result),
                ErrorCode.ValidationFailed => BadRequest(result),
                ErrorCode.CurrencyConversionFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPatch("items/{basketItemId}/quantity")]
        public async Task<ActionResult<ChangeItemQuantityResult>> ChangeItemQuantity(Guid basketItemId, [FromBody] ChangeItemQuantityCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            command.CustomerId = Guid.Parse(userId);
            command.BasketItemId = basketItemId;
            var result = await _changeItemQuantityHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.BasketNotFound => NotFound(result),
                ErrorCode.BasketItemNotFound => NotFound(result),
                ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutBasketResult>> Checkout()
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var command = new CheckoutBasketCommand { CustomerId = Guid.Parse(userId) };
            var result = await _checkoutBasketHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.BasketNotFound => NotFound(result),
                ErrorCode.BasketEmpty => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpDelete]
        public async Task<ActionResult<ClearBasketResult>> ClearBasket()
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Customer), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var command = new ClearBasketCommand { CustomerId = Guid.Parse(userId) };
            var result = await _clearBasketHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.BasketNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }
    }
}
