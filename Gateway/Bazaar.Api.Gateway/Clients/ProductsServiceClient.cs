using Bazaar.Api.Gateway.Requests.Products;
using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Api.Gateway.Clients
{
    public class ProductsServiceClient : BaseServiceClient
    {
        public ProductsServiceClient(
            IHttpClientFactory httpClientFactory,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(httpClientFactory, serviceTokenManager, httpContextAccessor,
                   configuration["Services:ProductsService"])
        {
        }

        public async Task<HttpResponseMessage> AddProduct(AddProductRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Post, "api/products", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetAllProducts()
        {
            var request = CreateRequest(HttpMethod.Get, "api/products", withUserContext: false);
            return await _httpClient.SendAsync(request);
        }
    }
}
