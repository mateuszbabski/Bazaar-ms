using Bazaar.Shared.Infrastructure.Auth;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Bazaar.Api.Gateway.Clients
{
    public abstract class BaseServiceClient
    {
        private readonly IServiceTokenManager _serviceTokenManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly HttpClient _httpClient;

        protected BaseServiceClient(
            IHttpClientFactory httpClientFactory,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor,
            string baseAddress)
        {
            _serviceTokenManager = serviceTokenManager;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
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
