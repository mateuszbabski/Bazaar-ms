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

        public async Task<HttpResponseMessage> GetProductById(Guid productId)
        {
            var request = CreateRequest(HttpMethod.Get, $"api/products/{productId}", withUserContext: false);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetProductsByShopId(Guid shopId)
        {
            var request = CreateRequest(HttpMethod.Get, $"api/products/shop/{shopId}", withUserContext: false);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SearchProducts(SearchProductsRequest requestObject)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(requestObject.Name))
                queryParams.Add($"name={Uri.EscapeDataString(requestObject.Name)}");
            if (!string.IsNullOrEmpty(requestObject.Category))
                queryParams.Add($"category={Uri.EscapeDataString(requestObject.Category)}");
            if (requestObject.MinPrice.HasValue)
                queryParams.Add($"minPrice={requestObject.MinPrice.Value}");
            if (requestObject.MaxPrice.HasValue)
                queryParams.Add($"maxPrice={requestObject.MaxPrice.Value}");

            var url = queryParams.Count > 0
                ? $"api/products/search?{string.Join("&", queryParams)}"
                : "api/products";

            var request = CreateRequest(HttpMethod.Get, url, withUserContext: false);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteProduct(Guid productId)
        {
            var request = CreateRequest(HttpMethod.Delete, $"api/products/{productId}", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UpdateProduct(UpdateProductRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, "api/products", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }
    }
}
