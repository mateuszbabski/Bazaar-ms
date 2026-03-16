using Bazaar.Api.Gateway.Requests.Customers;
using Bazaar.Shared.Infrastructure.Auth;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Bazaar.Api.Gateway.Clients
{
    public class CustomersServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceTokenManager _serviceTokenManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomersServiceClient(
            IHttpClientFactory httpClientFactory,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["Services:CustomersService"]);
            _serviceTokenManager = serviceTokenManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> SignUp(SignUpCustomerRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Post, "api/auth/signup", requestObject);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SignIn(SignInCustomerRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Post, "api/auth/signin", requestObject);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetCustomer()
        {
            var request = CreateRequest(HttpMethod.Get, "api/customers", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UpdateCustomer(UpdateCustomerRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, "api/customers/me", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UpdateCustomerToShop(UpdateCustomerToShopRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, "api/customers/me/updatetoshop", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        private HttpRequestMessage CreateRequest(
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
                    _httpClient.DefaultRequestHeaders.Add("X-User-Id", userId);
                }
                if (!string.IsNullOrEmpty(userRole))
                {
                    _httpClient.DefaultRequestHeaders.Add("X-User-Role", userRole);
                }
            }

            if (content is not null)
                request.Content = JsonContent.Create(content);

            return request;
        }
    }
}
