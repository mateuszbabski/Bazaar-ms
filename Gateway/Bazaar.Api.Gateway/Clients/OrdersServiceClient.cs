using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Api.Gateway.Clients
{
    public class OrdersServiceClient : BaseServiceClient
    {
        public OrdersServiceClient(
            IHttpClientFactory httpClientFactory,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(httpClientFactory, serviceTokenManager, httpContextAccessor,
                   configuration["Services:OrdersService"]!)
        {
        }

        public async Task<HttpResponseMessage> GetOrders(string? status)
        {
            var url = "api/orders";
            if (!string.IsNullOrEmpty(status))
                url += $"?status={status}";

            var request = CreateRequest(HttpMethod.Get, url, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetOrderById(Guid orderId)
        {
            var request = CreateRequest(HttpMethod.Get, $"api/orders/{orderId}", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PayForOrder(Guid orderId)
        {
            var request = CreateRequest(HttpMethod.Post, $"api/orders/{orderId}/pay", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> CancelOrder(Guid orderId)
        {
            var request = CreateRequest(HttpMethod.Patch, $"api/orders/{orderId}/cancel", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetShopOrders(string? status)
        {
            var url = "api/orders/shop";
            if (!string.IsNullOrEmpty(status))
                url += $"?status={status}";

            var request = CreateRequest(HttpMethod.Get, url, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }
    }
}
