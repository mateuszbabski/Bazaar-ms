using Bazaar.Api.Gateway.Requests.Baskets;
using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Api.Gateway.Clients
{
    public class BasketsServiceClient : BaseServiceClient
    {
        public BasketsServiceClient(
            IHttpClientFactory httpClientFactory,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(httpClientFactory, serviceTokenManager, httpContextAccessor,
                   configuration["Services:BasketsService"]!)
        {
        }

        public async Task<HttpResponseMessage> GetBasket()
        {
            var request = CreateRequest(HttpMethod.Get, "api/baskets", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> AddProductToBasket(AddProductToBasketRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Post, "api/baskets/items", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> RemoveProductFromBasket(Guid basketItemId)
        {
            var request = CreateRequest(HttpMethod.Delete, $"api/baskets/items/{basketItemId}", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> ChangeBasketCurrency(ChangeBasketCurrencyRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, "api/baskets/currency", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> ChangeItemQuantity(Guid basketItemId, ChangeItemQuantityRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, $"api/baskets/items/{basketItemId}/quantity", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Checkout()
        {
            var request = CreateRequest(HttpMethod.Post, "api/baskets/checkout", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> ClearBasket()
        {
            var request = CreateRequest(HttpMethod.Delete, "api/baskets", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }
    }
}
