using Bazaar.Api.Gateway.Clients;
using Bazaar.Api.Gateway.Requests.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly BasketsServiceClient _serviceClient;

        public BasketsController(BasketsServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetBasket()
        {
            var response = await _serviceClient.GetBasket();
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("items")]
        [Authorize]
        public async Task<ActionResult> AddProductToBasket([FromBody] AddProductToBasketRequest request)
        {
            var response = await _serviceClient.AddProductToBasket(request);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpDelete("items/{basketItemId}")]
        [Authorize]
        public async Task<ActionResult> RemoveProductFromBasket(Guid basketItemId)
        {
            var response = await _serviceClient.RemoveProductFromBasket(basketItemId);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("currency")]
        [Authorize]
        public async Task<ActionResult> ChangeBasketCurrency([FromBody] ChangeBasketCurrencyRequest request)
        {
            var response = await _serviceClient.ChangeBasketCurrency(request);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("items/{basketItemId}/quantity")]
        [Authorize]
        public async Task<ActionResult> ChangeItemQuantity(Guid basketItemId, [FromBody] ChangeItemQuantityRequest request)
        {
            var response = await _serviceClient.ChangeItemQuantity(basketItemId, request);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("checkout")]
        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            var response = await _serviceClient.Checkout();
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> ClearBasket()
        {
            var response = await _serviceClient.ClearBasket();
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
