using Bazaar.Baskets.Service.Commands.Dtos;
using Bazaar.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Bazaar.Baskets.Service.Commands.Clients
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IServiceTokenManager _serviceTokenManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductServiceClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            var baseAddress = _configuration["Services:ProductsService"];
            _serviceTokenManager = serviceTokenManager;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        public async Task<ProductDto?> GetProductById(Guid productId)
        {
            var request = CreateRequest(HttpMethod.Get, $"api/products/{productId}", withUserContext: false);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<ProductServiceResponse>();
            if (result is null || !result.IsSuccess || result.Product is null)
                return null;

            return result.Product;
        }

        protected HttpRequestMessage CreateRequest(
            HttpMethod method,
            string url,
            object? content = null,
            bool withUserContext = false)
        {
            var request = new HttpRequestMessage(method, url);

            var serviceToken = _serviceTokenManager.GenerateServiceToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceToken);

            if (withUserContext)
            {
                var claims = _httpContextAccessor.HttpContext?.User;
                var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = claims?.FindFirst(ClaimTypes.Role)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    request.Headers.Add("X-User-Id", userId);
                }
                if (!string.IsNullOrEmpty(userRole))
                {
                    request.Headers.Add("X-User-Role", userRole);
                }
            }

            if (content is not null)
                request.Content = JsonContent.Create(content);

            return request;
        }
    }
}
